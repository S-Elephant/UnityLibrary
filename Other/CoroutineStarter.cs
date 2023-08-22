#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Tracks and manages <see cref="Coroutine"/>s.
	/// You can also use this for starting unmanaged <see cref="Coroutine"/>s.
	/// </summary>
	/// <example>CoroutineStarter.Instance.StartCoroutine(MyCoroutine)</example>
	public class CoroutineStarter : MonoBehaviour
	{
		/// <summary>
		/// Used as a default value when no main- or sub-category is specified.
		/// </summary>
		public const string UncategorizedValue = "Uncategorized";

		/// <summary>
		/// Contains information about a tracked <see cref="Coroutine"/> and also contains the
		/// <see cref="Coroutine"/> itself.
		/// </summary>
		private struct CoroutineValue
		{
			/// <summary>
			/// Main-category, used for tracking.
			/// </summary>
			public string MainCategory { get; }

			/// <summary>
			/// Sub-category, used for tracking.
			/// </summary>
			public string SubCategory { get; }

			/// <summary>
			/// Optional tags, used for tracking.
			/// </summary>
			public string[] Tags { get; }

			/// <summary>
			/// The tracked <see cref="Coroutine"/>.
			/// </summary>
			public Coroutine Coroutine { get; }

			/// <summary>
			/// Constructor.
			/// </summary>
			public CoroutineValue(string mainCategory, string subCategory, Coroutine coroutine, string[] tags)
			{
				MainCategory = mainCategory;
				SubCategory = subCategory;
				Coroutine = coroutine;
				Tags = tags;
			}
		}

		/// <summary>
		/// Contains all tracked <see cref="Coroutine"/> data.
		/// </summary>
		private readonly Dictionary<string, CoroutineValue> _coroutines = new();

		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static CoroutineStarter? _instance = null;

		/// <summary>
		/// Public access singleton instance.
		/// </summary>
		public static CoroutineStarter Instance
		{
			get
			{
				// ReSharper disable once InvertIf
				if (_instance == null)
				{
					GameObject gameObject = new("CoroutineStarter");
					DontDestroyOnLoad(gameObject);
					_instance = gameObject.AddComponent<CoroutineStarter>();
					SceneManager.sceneUnloaded += SceneManagerOnSceneUnloaded;
				}

				return _instance;
			}
		}

		/// <summary>
		/// If true, will stop and untrack all <see cref="Coroutine"/>s when the scene is being unloaded.
		/// Defaults to false.
		/// </summary>
		public bool StopAllOnUnloadScene = false;

		/// <summary>
		/// All main-categories to stop and untrack when the scene is being unloaded.
		/// Defaults to none.
		/// </summary>
		public HashSet<string> MainCategoriesToStopOnUnloadScene = new();

		/// <summary>
		/// SceneUnloaded occurs after OnDisable and OnDestroy, but before the next Scene has been
		/// loaded (See end of this post for consequences of the event occurring after disable/destroy).
		/// </summary>
		private static void SceneManagerOnSceneUnloaded(Scene scene)
		{
			if (Instance.StopAllOnUnloadScene)
				Instance.StopAllManagedCoroutines();
			else
			{
				foreach (string mainCategoryToStop in Instance.MainCategoriesToStopOnUnloadScene)
					Instance.StopCoroutinesManaged(mainCategoryToStop);
			}
		}

		/// <summary>
		/// Destroy.
		/// </summary>
		private void OnDestroy()
		{
			try
			{
#pragma warning disable S2696 // Ignored because OnDestroy is a Unity event.
				_instance = null;
#pragma warning restore S2696
			}
			catch
			{
				// Do nothing.
			}

			try
			{
				SceneManager.sceneUnloaded -= SceneManagerOnSceneUnloaded;
			}
			catch
			{
				// Do nothing.
			}
		}

		/// <summary>
		/// Start a new tracked <see cref="Coroutine"/>.
		/// </summary>
		/// <param name="routine">The <see cref="Coroutine"/> function to start and track.</param>
		/// <param name="mainCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by main-category.</param>
		/// <param name="subCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by sub-category.</param>
		/// <param name="tags">Is used for tracking and stopping <see cref="Coroutine"/>s.</param>
		/// <returns>Started <see cref="Coroutine"/>.</returns>
		public Coroutine StartCoroutineManaged(IEnumerator routine, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, params string[] tags)
		{
			return StartCoroutineNamedManaged(routine, null, mainCategory, subCategory, false, tags);
		}

		/// <summary>
		/// Start a new tracked <see cref="Coroutine"/>.
		/// </summary>
		/// <param name="routine">The <see cref="Coroutine"/> function to start and track.</param>
		/// <param name="coroutineName">Unique name to identify each tracked <see cref="Coroutine"/>. If null then a random unique value will be assigned.</param>
		/// <param name="mainCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by main-category.</param>
		/// <param name="subCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by sub-category.</param>
		/// <param name="stopExisting">If true, will stop and remove an existing <see cref="Coroutine"/> with the same name as <paramref name="coroutineName"/>.</param>
		/// <param name="tags">Is used for tracking and stopping <see cref="Coroutine"/>s.</param>
		/// <returns>Started <see cref="Coroutine"/>.</returns>
		public Coroutine StartCoroutineNamedManaged(IEnumerator routine, string? coroutineName = null, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, bool stopExisting = true, params string[] tags)
		{
			if (coroutineName != null && _coroutines.ContainsKey(coroutineName))
			{
				if (stopExisting)
					Instance.StopCoroutine(_coroutines[coroutineName].Coroutine);
				else
					_coroutines.Remove(coroutineName);
			}
			else
			{
				coroutineName = $"NamelessCoroutine_{Guid.NewGuid()}";
			}

			Coroutine startedCoroutine = Instance.StartCoroutine(routine);
			_coroutines.Add(coroutineName, new CoroutineValue(mainCategory, subCategory, startedCoroutine, tags));

			return startedCoroutine;
		}

		/// <summary>
		/// Stops the coroutine and stops tracking it.
		/// Note that if the specified key does not exist then nothing will happen.
		/// </summary>
		/// <param name="coroutineName">Unique name of the tracked coroutine to stop.</param>
		/// <returns>true if stopped and removed.</returns>
		public bool StopCoroutineManaged(string coroutineName)
		{
			if (!_coroutines.TryGetValue(coroutineName, out CoroutineValue coroutineToStop))
				return false;

			StopCoroutine(coroutineToStop.Coroutine);
			_coroutines.Remove(coroutineName);
			return true;
		}

		/// <summary>
		/// Stops and untracks all tracked coroutines.
		/// </summary>
		/// <returns>Amount stopped.</returns>
		public int StopAllManagedCoroutines()
		{
			int count = _coroutines.Count;

			foreach (var kvp in _coroutines)
				StopCoroutine(kvp.Value.Coroutine);
			_coroutines.Clear();

			return count;
		}

		/// <summary>
		/// Stops the <see cref="Coroutine"/>s that match any of the the input parameters that act as filters.
		/// Note that if ANY of the input parameters are matched with a tracked <see cref="Coroutine"/> then it will
		/// be stopped and untracked.
		/// </summary>
		/// <param name="mainCategory">
		/// If not null, will only stop <see cref="Coroutine"/>s that match this main-category. If a <paramref name="subCategory"/>
		/// is also supplied then it must also match that sub-category.
		/// </param>
		/// <param name="subCategory">
		/// If not null, will only stop <see cref="Coroutine"/>s that match this sub-category. If a <paramref name="mainCategory"/>
		/// is also supplied then it must also match that main-category.
		/// </param>
		/// <param name="tags">Will stop <see cref="Coroutine"/> that match any of the supplies tags. This ignores
		/// <paramref name="mainCategory"/> and <paramref name="subCategory"/> when matching.</param>
		/// <returns>Names of all stopped <see cref="Coroutine"/>s.</returns>
		public List<string> StopCoroutinesManaged(string? mainCategory = null, string? subCategory = null, List<string>? tags = null)
		{
			if (tags?.Count == 0)
				tags = null;

			// Do nothing if there's nothing to stop.
			if (mainCategory == null && subCategory == null && tags == null)
				return new List<string>();

			// Retrieve which keys to stop.
			HashSet<string> coroutineKeysToStop = new();
			foreach (var kvp in _coroutines)
			{
				string name = kvp.Key;
				CoroutineValue coroutineValue = kvp.Value;

				// Process filtering by categories.
				if (mainCategory != null || subCategory != null)
				{
					if (mainCategory == null && subCategory != null)
					{
						// Main-category is not specified but the sub-category is. Therefore add if it matches any sub-category, regardless of its main-category.
						if (coroutineValue.SubCategory == subCategory)
							coroutineKeysToStop.Add(name);
					}
					else
					{
						// Filter by main-category first.
						if (coroutineValue.MainCategory == mainCategory)
						{
							if (subCategory == null)
							{
								// Main-category filters exists but no sub-category, therefore, add the coroutine if it matches the main-category, regardless of the sub-category.
								coroutineKeysToStop.Add(name);
							}
							else
							{
								// Main-category and sub-category exists, therefore, add the coroutine if it matches both the main-category and sub-category.
								if (coroutineValue.SubCategory == subCategory)
									coroutineKeysToStop.Add(name);
							}
						}
					}
				}

				// Process filtering by tags. Adds if any tags match.
				if (tags != null && coroutineValue.Tags.Intersect(tags).Any())
					coroutineKeysToStop.Add(name);
			}

			// Stop and untrack all matched.
			foreach (string coroutineKeyToStop in coroutineKeysToStop)
				StopCoroutineManaged(coroutineKeyToStop);

			return coroutineKeysToStop.ToList();
		}
	}
}