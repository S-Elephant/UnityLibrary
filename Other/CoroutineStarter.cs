#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Tracks and manages <see cref="Coroutine"/>s.
	/// You can also use this for starting unmanaged/untracked <see cref="Coroutine"/>s.
	/// </summary>
	/// <example>CoroutineStarter.Instance.StartCoroutine(MyCoroutine)</example>
	public class CoroutineStarter : MonoBehaviour
	{
		/// <summary>
		/// Used as a default value when no main- or sub-category is specified.
		/// </summary>
		public const string UncategorizedValue = "Uncategorized";

		/// <summary>
		/// Is called after a <see cref="Coroutine"/> is being tracked.
		/// </summary>
		public event EventHandler<CoroutineValue>? OnStartTrackingCoroutine = null;

		/// <summary>
		/// Is called after a <see cref="Coroutine"/> is no longer being tracked.
		/// </summary>
		public event EventHandler<CoroutineValue>? OnStopTrackingCoroutine = null;

		/// <summary>
		/// Contains information about a tracked <see cref="Coroutine"/> and also contains the
		/// <see cref="Coroutine"/> itself.
		/// </summary>
		public struct CoroutineValue
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
			/// Name of the calling method.
			/// </summary>
			public string CallerName { get; }

			/// <summary>
			/// Triggers after <see cref="Coroutine"/> that belongs to this container finished executing.
			/// </summary>
			public Action? OnComplete;

			/// <summary>
			/// Constructor.
			/// </summary>
			public CoroutineValue(string mainCategory, string subCategory, Coroutine coroutine, string callerName, Action? onComplete, string[] tags)
			{
				MainCategory = mainCategory;
				SubCategory = subCategory;
				Coroutine = coroutine;
				CallerName = callerName;
				Tags = tags;
				OnComplete = onComplete;
			}
		}

		/// <summary>
		/// Contains all tracked <see cref="Coroutine"/> data.
		/// The coroutine name is the key and the value contains the data.
		/// </summary>
		public readonly Dictionary<string, CoroutineValue> CoroutineData = new();

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
		/// If true, will stop and untrack all tracked <see cref="Coroutine"/>s when the scene is being unloaded.
		/// Defaults to false.
		/// </summary>
		public bool StopAllTrackedOnUnloadScene = false;

		/// <summary>
		/// All main-categories to stop and untrack when the scene is being unloaded.
		/// Defaults to none.
		/// </summary>
		public HashSet<string> MainCategoriesToStopOnUnloadScene = new();

		/// <summary>
		/// Total amount of tracked <see cref="Coroutine"/>s that were started.
		/// </summary>
		public static int TotalTrackedCoroutinesStarted { get; private set; } = 0;

		/// <summary>
		/// Total amount of tracked <see cref="Coroutine"/>s that were stopped.
		/// </summary>
		public static int TotalTrackedCoroutinesStopped { get; private set; } = 0;

		/// <summary>
		/// SceneUnloaded occurs after OnDisable and OnDestroy, but before the next Scene has been
		/// loaded (See end of this post for consequences of the event occurring after disable/destroy).
		/// </summary>
		private static void SceneManagerOnSceneUnloaded(Scene scene)
		{
			if (Instance.StopAllTrackedOnUnloadScene)
				Instance.StopAllTrackedCoroutines();
			else
			{
				foreach (string mainCategoryToStop in Instance.MainCategoriesToStopOnUnloadScene)
					Instance.StopTrackedCoroutines(mainCategoryToStop);
			}
		}

		/// <summary>
		/// Destroy.
		/// </summary>
#pragma warning disable IDE0051 // Ignored because OnDestroy is a Unity event.
		private void OnDestroy()
#pragma warning restore IDE0051
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
		/// Returns true if a <see cref="Coroutine"/> with the name <paramref name="coroutineName"/> (case-sensitive)
		/// is currently being tracked.
		/// </summary>
		/// <param name="coroutineName">The name of the <see cref="Coroutine"/> to check if it's being tracked.</param>
		/// <returns>
		/// true if a <see cref="Coroutine"/> with the name <paramref name="coroutineName"/> (case-sensitive) is currently being tracked.
		/// </returns>
		public virtual bool IsTrackingCoroutineWithName(string coroutineName)
		{
			return CoroutineData.ContainsKey(coroutineName);
		}

		/// <summary>
		/// Start a new tracked <see cref="Coroutine"/>.
		/// </summary>
		/// <param name="routine">The <see cref="Coroutine"/> function to start and track.</param>
		/// <param name="mainCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by main-category.</param>
		/// <param name="subCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by sub-category.</param>
		/// <param name="callerName">Calling method its name.</param>\
		/// <param name="onComplete">Optional action to execute when this tracked <see cref="Coroutine"/> completes.</param>
		/// <param name="tags">Is used for tracking and stopping <see cref="Coroutine"/>s.</param>
		/// <returns>Started <see cref="Coroutine"/>.</returns>
		/// <remarks>Func is required to prevent reusing the same <see cref="IEnumerator"/>. This creates a new <see cref="IEnumerator"/> instead.</remarks>
#pragma warning disable S3343
		public virtual Coroutine StartCoroutineTracked(Func<IEnumerator> routine, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, [CallerMemberName] string callerName = "", Action? onComplete = null, params string[] tags)
#pragma warning restore S3343
		{
			return StartCoroutineNamedTracked(routine, null, mainCategory, subCategory, false, callerName, onComplete, tags);
		}

		/// <summary>
		/// Start a new tracked <see cref="Coroutine"/>.
		/// </summary>
		/// <param name="routine">The <see cref="Coroutine"/> function to start and track.</param>
		/// <param name="coroutineName">Unique name to identify each tracked <see cref="Coroutine"/>. If null then a random unique value will be assigned.</param>
		/// <param name="mainCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by main-category.</param>
		/// <param name="subCategory">Is used for tracking and stopping <see cref="Coroutine"/>s by sub-category.</param>
		/// <param name="stopExisting">If true, will stop and remove an existing <see cref="Coroutine"/> with the same name as <paramref name="coroutineName"/>; otherwise, will untrack it but not stop it.</param>
		/// <param name="callerName">Calling method its name.</param>\
		/// <param name="onComplete">Optional action to execute when this tracked <see cref="Coroutine"/> completes.</param>
		/// <param name="tags">Is used for tracking and stopping <see cref="Coroutine"/>s.</param>
		/// <returns>Started <see cref="Coroutine"/>.</returns>
		/// <remarks>Func<> is required to prevent reusing the same <see cref="IEnumerator"/>. This creates a new <see cref="IEnumerator"/> instead.</remarks>
#pragma warning disable S3343
		public virtual Coroutine StartCoroutineNamedTracked(Func<IEnumerator> routine, string? coroutineName = null, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, bool stopExisting = true, [CallerMemberName] string callerName = "", Action? onComplete = null, params string[] tags)
#pragma warning restore S3343
		{
			if (string.IsNullOrEmpty(coroutineName))
			{
				do
				{
					coroutineName = $"NamelessCoroutine_{Guid.NewGuid()}";
				}
				while (IsTrackingCoroutineWithName(coroutineName)); // Because there's an astronomically small chance that the GUID is not unique.
			}
			else
			{
				// Handle existing co-routine.
				if (IsTrackingCoroutineWithName(coroutineName!))
				{
					if (stopExisting)
						StopTrackedCoroutine(coroutineName!);
					else
						RemoveCoroutineByName(coroutineName!); // Don't stop but but stop tracking it instead.
				}
			}

			Coroutine actualStartedCoroutine = Instance.StartCoroutine(InternalCoroutine(routine(), coroutineName!, onComplete)); // Note that the routine() parameter also starts the routine.
			CoroutineValue newCoroutineValue = new(mainCategory, subCategory, actualStartedCoroutine, callerName, onComplete, tags);
			CoroutineData.Add(coroutineName!, newCoroutineValue);
			OnStartTrackingCoroutine?.Invoke(this, newCoroutineValue);
			TotalTrackedCoroutinesStarted++;

			return actualStartedCoroutine;
		}

		/// <summary>
		/// This <see cref="Coroutine"/> wrapper is used to detect when the <paramref name="actualStartedRoutine"/> finished.
		/// </summary>
		private IEnumerator InternalCoroutine(IEnumerator actualStartedRoutine, string coroutineName, Action? onComplete)
		{
			yield return actualStartedRoutine;
			RemoveCoroutineByName(coroutineName);
			onComplete?.Invoke();
		}

		/// <summary>
		/// Stops the coroutine and stops tracking it.
		/// Note that if the specified key does not exist then nothing will happen.
		/// </summary>
		/// <param name="coroutineName">Unique name of the tracked coroutine to stop.</param>
		/// <returns>true if stopped and removed.</returns>
		public virtual bool StopTrackedCoroutine(string coroutineName)
		{
			if (!CoroutineData.TryGetValue(coroutineName, out CoroutineValue coroutineToStop))
				return false;

			if (coroutineToStop.Coroutine == null) // I'm not sure why but it can be null.
				StopCoroutine(coroutineToStop.Coroutine);
			RemoveCoroutineByName(coroutineName);
			return true;
		}

		/// <summary>
		/// Removes the specified coroutineValue by key name from <see cref="CoroutineData"/> and
		/// fires <see cref="OnStopTrackingCoroutine"/> if it was removed.
		/// Will do nothing if it wasn't found or if it wasn't removed.
		/// Does not stop the <see cref="Coroutine"/> itself.
		/// </summary>
		private void RemoveCoroutineByName(string coroutineName, [CallerMemberName] string callerName = "")
		{
			if (coroutineName == string.Empty)
			{
				Debug.LogError($"Tried to remove a tracked coroutine with an empty key. Aborting removal. Was called by: {callerName}");
				return;
			}

			bool exists = CoroutineData.TryGetValue(coroutineName, out CoroutineValue removedCoroutineValue);
			if (!exists)
				return;

			bool isRemoved = CoroutineData.Remove(coroutineName);

			if (isRemoved)
			{
				OnStopTrackingCoroutine?.Invoke(this, removedCoroutineValue);
				TotalTrackedCoroutinesStopped++;
			}
		}

		/// <summary>
		/// Stop tracking the <see cref="Coroutine"/> with the <paramref name="coroutineName"/> name.
		/// Does not stop the <see cref="Coroutine"/> itself.
		/// </summary>
		/// <param name="coroutineName">Name of the tracked <see cref="Coroutine"/> to stop tracking.</param>
		/// <returns>true if tracking was stopped.</returns>
		public virtual bool StopTracking(string coroutineName)
		{
			if (!CoroutineData.ContainsKey(coroutineName))
				return false;

			RemoveCoroutineByName(coroutineName);
			return true;
		}

		/// <summary>
		/// Stops and untracks all tracked coroutines.
		/// Triggers <see cref="OnStopTrackingCoroutine"/> for each tracked coroutine after stopping them all.
		/// </summary>
		/// <returns>Amount stopped.</returns>
		public virtual int StopAllTrackedCoroutines()
		{
			int count = CoroutineData.Count;

			foreach (var kvp in CoroutineData)
			{
				StopCoroutine(kvp.Value.Coroutine);
				RemoveCoroutineByName(kvp.Key);
			}

			CoroutineData.Clear();

			return count;
		}

		/// <summary>
		/// Stops and untracks all tracked and untracked coroutines anywhere in Unity.
		/// Use with caution.
		/// Triggers <see cref="OnStopTrackingCoroutine"/> for each tracked coroutine after stopping all <see cref="Coroutine"/>s.
		/// </summary>
		public new void StopAllCoroutines()
		{
			base.StopAllCoroutines();

			foreach (var kvp in CoroutineData)
				RemoveCoroutineByName(kvp.Key);

			CoroutineData.Clear();
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
		public virtual List<string> StopTrackedCoroutines(string? mainCategory = null, string? subCategory = null, List<string>? tags = null)
		{
			if (tags?.Count == 0)
				tags = null;

			// Do nothing if there's nothing to stop.
			if (mainCategory == null && subCategory == null && tags == null)
				return new List<string>();

			// Retrieve which keys to stop.
			HashSet<string> coroutineKeysToStop = new();
			foreach (var kvp in CoroutineData)
			{
				string name = kvp.Key;
				CoroutineValue coroutineValue = kvp.Value;

				// Process filtering by categories.
				if (mainCategory != null || subCategory != null)
				{
					if (mainCategory == null)
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
				StopTrackedCoroutine(coroutineKeyToStop);

			return coroutineKeysToStop.ToList();
		}
	}
}