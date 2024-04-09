#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Elephant.UnityLibrary.Common;
using UnityEngine;

namespace Elephant.UnityLibrary.Localizations
{
	/// <summary>
	/// <para>Manages localization by loading and providing access to localized text strings.</para>
	/// <para>Loads the localized data BEFORE the scene loads. You can safely call
	/// LocalizationManager.Instance.Localize in your Awake().</para>
	/// </summary>
	/// <example>
	/// <para>Usage instructions: Attach this to a <see cref="GameObject"/> and put your CSV files into
	/// "Assets/Resources/". Example: "Assets/Resources/Localizations/Localization_en.csv" for English.</para>
	///
	/// <para>The first line of the CSV file is the header. You can put here anything you'd like because this line
	/// is ignored.</para>
	///
	/// <para>CSV content example:<br/>
	/// Key,Translation<br/>
	/// musicVolume,music volume<br/>
	/// soundVolume,sound volume</para>
	///
	/// <para>You may override this class and use your own class if you need to change settings or behaviour
	/// like for example the <see cref="DefaultLanguageKey"/>.</para>
	/// </example>
	public class LocalizationManager : MonoBehaviour
	{
		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static LocalizationManager Instance = null!;

		/// <summary>
		/// Represents the method that will handle an event after the language changed.
		/// </summary>
		public delegate void OnLanguageChangedEventHandler();

		/// <summary>
		/// Occurs after the language changed.
		/// </summary>
		public event OnLanguageChangedEventHandler? OnLanguageChanged = null;

		/// <summary>
		/// Stores localization data. The outer dictionary's key is the text key, and the inner dictionary's key is the language code.
		/// </summary>
		private Dictionary<string, Dictionary<string, string>> _localizationData = new();

		/// <summary>
		/// Current language code (e.g., "en" for English).
		/// </summary>
		public string CurrentLanguageKey { get; private set; } = string.Empty;

		/// <summary>
		/// Initializes the localization system before any scene is loaded.
		/// This method is automatically called by Unity.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			GameObject managerGameObject = new("LocalizationManager");

			// Assign or create an instance of this class.
			Instance = FindFirstObjectByType<LocalizationManager>();
			if (Instance == null)
				Instance = managerGameObject.AddComponent<LocalizationManager>();

			Instance.SetLanguage(Instance.DefaultLanguageKey());
		}

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
			if (Instance != null && Instance != this)
				Destroy(gameObject); // Ensures there's only one instance.

			DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// Default language key.
		/// This will determine the default language.
		/// </summary>
		protected virtual string DefaultLanguageKey() => "en";

		/// <summary>
		/// CSV separator char.
		/// </summary>
		public virtual char SeparatorChar() => ',';

		/// <summary>
		/// Method that returns the localization filename (excluding extension), likely based on the input parameter.
		/// </summary>
		/// <param name="languageKey">Language code to set as the current language. (e.g., "en" for English). Is case-insensitive.</param>
		/// <returns>Localization filename (including extension) based on the <paramref name="languageKey"/>.</returns>
		protected virtual string BuildDirectoriesAndFilename(string languageKey)
		{
			return $"Localizations/Localization_{languageKey}";
		}

		/// <summary>
		/// Retrieve the CSV lines, excluding the CSV header line,
		/// removing empty lines and lines that contain only spaces.
		/// </summary>
		protected virtual List<string> LinesFromTextAsset(TextAsset csvFile)
		{
			string[] lines = csvFile.text.Split('\n');

			if (!lines.Any())
				return new List<string>();

			// Optimistically allocate space for all lines.
			List<string> resultList = new List<string>(lines.Length - 1);

			// Start from index 1 to skip the first line (which would be the CSV header line).
			for (int i = 1; i < lines.Length; i++)
			{
				string trimmedLine = lines[i].Trim();
				if (trimmedLine != string.Empty)
				{
					// Add only non-empty, trimmed lines.
					resultList.Add(trimmedLine);
				}
			}

			return resultList;
		}

		/// <summary>
		/// Return the columns from a CSV line.
		/// Supports CSV rows escaped with double quotes.
		/// </summary>
		protected virtual string[] ColumnsFromCsvLine(string csvLine)
		{
			// Split on commas that are not inside double quotes.
			Regex csvSplit = new(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
			string[] columns = csvSplit.Split(csvLine);

			// Further process each element to remove leading/trailing quotes if necessary.
			for (int i = 0; i < columns.Length; i++)
			{
				columns[i] = columns[i].Trim(); // Trim whitespace.

				if (columns[i].StartsWith("\"") && columns[i].EndsWith("\""))
				{
					// Remove leading and trailing quotes.
					columns[i] = columns[i].Substring(1, columns[i].Length - 2);
				}

				// Replace any double double-quotes with a single double-quote.
				columns[i] = columns[i].Replace("\"\"", "\"");
			}

			return columns;
		}

		/// <summary>
		/// Loads localization data from a CSV file.
		/// </summary>
		public virtual void LoadLocalizationFile()
		{
			// Get the CSV lines.
			string filename = BuildDirectoriesAndFilename(CurrentLanguageKey); // CSV file example: "Assets/Resources/Localizations/Example_en.csv".
			TextAsset? csvTextAsset = Resources.Load<TextAsset>(filename);
			if (csvTextAsset == null)
				return; // Failed to load CSV file. Aborting.
			List<string> lines = LinesFromTextAsset(csvTextAsset);
			char separator = SeparatorChar();

			for (int i = 0; i < lines.Count; i++)
			{
				string[] cells = ColumnsFromCsvLine(lines[i]);
				string key = cells[0].ToLowerInvariant();
				for (int j = 1; j < cells.Length; j++)
				{
					// Create the language key in the _localizationData if it doesn't already exist.
					if (!_localizationData.ContainsKey(key))
						_localizationData[key] = new Dictionary<string, string>();

					_localizationData[key][CurrentLanguageKey] = cells[j].ToLowerInvariant();
				}
			}
		}

		/// <summary>
		/// Sets the current language for localization.
		/// </summary>
		/// <param name="languageKey">Language code to set as the current language. (e.g., "en" for English). Is case-insensitive.</param>
		public virtual void SetLanguage(string languageKey)
		{
			languageKey = languageKey.ToLowerInvariant();
			if (CurrentLanguageKey == languageKey)
				return;

			CurrentLanguageKey = languageKey;
			LoadLocalizationFile();
			OnLanguageChanged?.Invoke();
		}

		/// <summary>
		/// Retrieves a localized string by key for the current language.
		/// </summary>
		/// <param name="key">Key for the desired text. Is case-insensitive.</param>
		/// <param name="capitalizeFirstChar">If true, capitalizes the first character of the localized result.</param>
		/// <returns>Localized string if found; otherwise, a warning message indicating the key is missing.</returns>
		public virtual string Localize(string key, bool capitalizeFirstChar = false)
		{
			key = key.ToLowerInvariant();
			if (_localizationData.ContainsKey(key) && _localizationData[key].ContainsKey(CurrentLanguageKey))
			{
				string result = _localizationData[key][CurrentLanguageKey];
				return capitalizeFirstChar ? StringOperations.CapitalizeFirstChar(result) : result;
			}

			if (_localizationData.ContainsKey(key))
				Debug.LogError($"Missing localization language language [{CurrentLanguageKey}]");
			else
				Debug.LogError($"Missing localization key for language [{CurrentLanguageKey}] and key: {key}");

			return capitalizeFirstChar ? StringOperations.CapitalizeFirstChar(key) : key;
		}
	}
}