# Unity library

Written for Unity Editor version **2022.3.7f1 (LTS)**. Note that most, if not all, earlier versions also seem to work but it's not recommend using this library in Unity Editor versions that are really old.

# Installation

Copy all Folders into your project. <u>Don't</u> put it in special Unity folders like "Editor" or "Resources".

If you use the DLL version instead then ensure that the DLL file is placed in the "Plugins" folder, which is a special Unity folder. If this folder does not exist then first create a new folder in your project root called "Plugins" and then add the DLL file into that folder.

The DLL "Elephant.UnityLibrary.Localizations.dll" is contains optional localization code, you don't need this DLL if you don't use my localization code.


# CoroutineStarter

Allows you to start coroutines from non-monobehaviour code and can optionally also manage and track coroutines because Unity does not support a CancellationToken. Supports an OnCompleted event, has categories and tags. Example screenshot:

![CoroutineStarter screenshot](ReadmeResources/CoroutineStarter.jpg)

## Example

```c#
private IEnumerator WaitMe(float seconds)
{
	yield return new WaitForSeconds(seconds);
	Debug.Log($"Done waiting for {seconds} seconds.");
}

private void SomeOptionalCallback()
{
	Debug.Log("Finished.");
}

private void Awake()
{
	CoroutineStarter.Instance.StartCoroutineManaged(WaitMe(2, SomeOptionalCallback), "Debug", "Example");
}

private void Start()
{
	CoroutineStarter.Instance.StopCoroutinesManaged("Debug"); // Stops all coroutines in the "Debug" main category.
	CoroutineStarter.Instance.StopCoroutinesManaged("Debug", "Example"); // Stops all coroutines in the "Debug" main-category that also are located in the "Example" sub-category.
}
```

## Methods, properties and functions

```c#
// Events.
public event EventHandler<CoroutineValue>? OnStartTrackingCoroutine;
public event EventHandler<CoroutineValue>? OnStopTrackingCoroutine;

// Methods and properties.
CoroutineStarter.UncategorizedValue // Used as a default value when no main- or sub-category is specified.
CoroutineStarter.Instance // Access the CoroutineStarter its singleton instance.    
CoroutineStarter.StopAllTrackedOnUnloadScene // If true, will stop and untrack all coroutiness when the scene is being unloaded. Defaults to false.
CoroutineStarter.MainCategoriesToStopOnUnloadScene // All main-categories to stop and untrack when the scene is being unloaded. Defaults to none.
CoroutineStarter.TotalTrackedCoroutinesStarted;
CoroutineStarter.TotalTrackedCoroutinesStopped;

// Public functions.
public virtual Coroutine StartCoroutineTracked(Func<IEnumerator> routine, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, [CallerMemberName] string callerName = "", Action? onComplete = null, params string[] tags);

public virtual Coroutine StartCoroutineNamedTracked(Func<IEnumerator> routine, string? coroutineName = null, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, bool stopExisting = true, [CallerMemberName] string callerName = "", Action? onComplete = null, params string[] tags);

public virtual bool StopTrackedCoroutine(string coroutineName);

public virtual bool StopTracking(string coroutineName);

public virtual int StopAllTrackedCoroutines();

public new void StopAllCoroutines();

public virtual List<string> StopTrackedCoroutines(string? mainCategory = null, string? subCategory = null, List<string>? tags = null);
```

# Menus

They are located under "Tools > Elephant".

![Render normals example](ReadmeResources/Menu.jpg)


# Loggers

## Example implementation of the non-static variant

```c#
public class PokemonLogger : ConsoleLogger
{
    public PokemonLogger()
    {
        // Optionally assign a tag to this logger.
        LogTag = "Pokemon";

#if !DEBUG
        // Example of setting a logging filter in release builds. This is optional.
        FilterLogType = LogType.Warning;
#endif
    }

    // Optionally override this method to determine when a specific logger should be allowed to log.
    public override bool IsLogTypeAllowed(LogType logType)
    {
        return base.IsLogTypeAllowed(logType) && <Whatever condition you want>;
    }
}

// Put this class onto a GameObject prefab and put that one into your special Unity's "Resources" folder.
public class LogManager : MonoBehaviour
{
	public static LogManager Instance = null!;

	public PokemonLogger PokemonLogger { get; } = new();

	/// <summary>
	/// Runs before a scene gets loaded and will only be executed once.
	/// </summary>
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize()
	{
		PokemonLogger = new()
	}

	public override void Initialize()
	{
		Instance = this;

		// Allows all console log types globally.
		Debug.unityLogger.filterLogType = LogType.Log;

         // Allows console logging globally.
		Debug.unityLogger.logEnabled = true;
	}
}

// Now somewhere in your code:
private PokemonLogger _logger = null!;
private void Awake()
{
	_logger = LogManager.Instance.FfseLogger;
    // Note that double clicking this log in the Unity console should open the below line in your IDE as well.
	_logger.Log("A yellow mouse entered your code. Prepare for an epic battle!")
}
```

# Extensions

## Between

```c#
public static bool IsBetween<T>(this T value, T min, T max)	where T : IComparable<T>
public static bool IsBetweenII<T>(this T value, T min, T max) where T : IComparable<T>
public static bool IsBetweenEI<T>(this T value, T min, T max) where T : IComparable<T>
public static bool IsBetweenIE<T>(this T value, T min, T max) where T : IComparable<T>
public static bool IsBetweenEE<T>(this T value, T min, T max) where T : IComparable<T>
```

## Enumerable

```c#
// Return random element(s).
public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> source, int count);

// Return random element.
public static T GetRandom<T>(this IEnumerable<T> source);

// Shuffle the elements. Modifies the source.
public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source);

// Determines if the source is empty.
public static bool IsEmpty<TSource>(this IEnumerable<TSource> source);

// Return true if ALL of values are contained in source.
// Returns true if values is empty.
public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> values);

// Return true if NONE of values are contained in source.
// Returns true if either source or values is empty.
public static bool ContainsNone<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> values);

// Determines if the <paramref name="source"/> is empty.
public static bool None<TSource>(this IEnumerable<TSource> source);

// Determines whether any element of a sequence does not satisfy a condition. This is the same as !source.Any(..).
public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

// Return true if the first item of source equals itemToCompare.
public static bool IsFirst<TSource>(this IEnumerable<TSource> source, TSource itemToCompare);

// Return true if the last item of source equals itemToCompare.
public static bool IsLast<TSource>(this IEnumerable<TSource> source, TSource itemToCompare);

// Return true if EVERY item in source is unique or if it is empty or null.
public static bool AreAllItemsUnique<TSource>(this IEnumerable<TSource>? source);
```

## List

```c#
// Shuffle the elements. Modifies the source list.
public static void Shuffle<T>(this IList<T> list);

// Add itemToAdd only if it doesn't already exist in list.
public static void AddIfNotExists<T>(this List<T> list, T itemToAdd);

// Add the item to the source list unless it already exists in that list in which case it will remove it instead. Modifies the source list.
public static IList<TSource> AddOrRemoveIfExists<TSource>(this IList<TSource> list, TSource item);

// Add the item to the <paramref name="list"/> unless it already exists in that list in which case it will remove it instead.
// If <paramref name="list"/> is null then nothing happens. Modifies the source list.
public static IList<TSource>? AddOrRemoveIfExistsNullable<TSource>(this IList<TSource>? list, TSource item);
```



## General

```c#
// Calculates the normalized direction from one vector to another.
public static Vector3 DirectionTo(this Vector3 from, Vector3 to);

// Returns the orthographic camera bounds in world space.
public static Bounds OrthographicBounds(this Camera camera);

// Searches recursively for a child transform with the specified name within the descendants of the given parent transform.
public static Transform FindRecursively(this Transform parent, string name);
```

## Rect

```c#
// Calculates the combined Axis-Aligned Bounding Box (AABB) from a collection
// of Rect objects. This method determines the minimum and maximum extents of
// the collection and returns a Rect that encompasses all the Rect objects within
// it. If the collection is null or empty, a Rect with zero position and size is
// returned.
public static Rect Combine(this IEnumerable<Rect>? rects);
```

## Wrapping

```c#
// Keeps the value between min and max. If it exceeds max then it'll start over at min again (keeping the overflow value).
public static int Wrap(this int value, int max, int min = 0);

// Keeps the value between 1 and max. If it exceeds max then it'll start over at min again (keeping the overflow value).
public static int WrapOne(this int value, int max);

// Keeps the value between min and max. If it exceeds max then it'll start over at min again (keeping the overflow value).
public static int? Wrap(this int? value, int max, int min = 0);

// Keeps the value between 1 and max. If it exceeds max then it'll start over at min again (keeping the overflow value).
public static int? WrapOne(this int? value, int max);
```



# Unity objects

## Camera

- FlyCam
- ResolutionIndependentCamera
- ScaleCameraWithScreenResolution

## Core

- Singleton
- SingletonNonPersistent

## Debugging (only works if you don't use the DLL version)

- RenderNormals (for testing normals, shadows, etc.)
  - RenderNormalsData

![Render normals example](ReadmeResources/RenderNormalsExample.jpg)
![Render normals inspector](ReadmeResources/RenderNormalsInspector.jpg)

## Editor

- AutoOpenAndFocusConsole (Only works if you use the non-DLL version).

## Various

And various others but I'm too lazy to document those right now.


# GeoSystems

Is able to render complex (Multi)-Polygons.

![Complex polygon example](ReadmeResources/ComplexPolygonExample.jpg)

<p float="left">
  <img src="ReadmeResources/ComplexPolygonExample2.jpg" width="48%" />
  <img src="ReadmeResources/ComplexPolygonExampleInspector.jpg" width="48%" />
</p>

## Geometry object Classes

- **GeometryVertex**
  - Is just a coordinate.
- **GeometryLine**
  - Contains 2 vertices.
- **Ring**
  - Contains 0 or more lines.
- **Polygon**
  - Contains 0 or 1 exterior ring and 0 or more interior rings.
- **MultiPolygon**
  - Contains zero or more polygons.

## Geometry renderers

- **DynamicMeshLinesRenderer**
  - **DynamicMeshLinesRendererEditor** (editor only)
- **GeometryFillRenderer**
- **MultiPolygonRenderer**

## Helpers

### WktPolygonParser

```c#
// Parse a WKT string representing either a POLYGON or a MULTIPOLYGON.
public static List<List<List<Vector2>>> ParseWkt(string? wkt);

// Splits a multi-polygon string into polygon strings.
public static List<string> SplitMultiPolygonIntoPolygons(string multiPolygonContent)

// Returns true if the geometry is truly a multi-polygon (instead of a regular polygon).
public static bool IsMultiPolygon(List<List<List<Vector2>>> geometry)

// Convert geometry into either a POLYGON or MULTIPOLYGON WKT string.
public static string ToWktString(List<List<List<Vector2>>> geometry, bool forceAsMultiPolygon = false, string defaultValue = EmptyMultiPolygon)
```

### WktPolygonUtils

```c#
/// Calculate bounds of a multi-polygon its points.
public static (Vector2 MinBounds, Vector2 MaxBounds) PointsToBounds(List<List<List<Vector2>>> geometry);

// Calculate center vector from min- and max bounds.
public static Vector2 CenterFromBounds(Vector2 minBounds, Vector2 maxBounds);

// Rotates a complex geometry consisting of multiple polygons.
public static List<List<List<Vector2>>> RotatePoints(List<List<List<Vector2>>> geometry, float degrees, Vector2 origin)

// Rotates a geometry represented by a WKT (Well-Known Text) string.
public static List<List<List<Vector2>>> RotateWktStringAsPoints(string wktString, float degrees, Vector2 origin);

// Rotates a geometry represented by a WKT string and returns the result as a WKT string.
public static string RotateWktString(string wktString, float degrees, Vector2 origin)

// Normalizes the degrees to be within 0 to 359.99999.
public static float NormalizeDegrees(float degrees);

// Translate (=move its position) the wktString.
public static string Translate(string wktString, Vector2 translation);

// Translate (=move its position) the geometry.
public static List<List<List<Vector2>>> Translate(List<List<List<Vector2>>> geometry, Vector2 translation);

// Calculates the area of a polygonal ring using the Shoelace formula.
// WARNING: This method is VERY inaccurate! A better implementation is perhaps required.
public static float CalculateRingArea(List<Vector2> ring);

// Calculates the area of a multi-polygon using the Shoelace formula.
// WARNING: This method is VERY inaccurate! A better implementation is perhaps required.
public static float CalculateSurfaceArea(List<List<List<Vector2>>> multipolygon);
```

# Localization

- Use namespace **Elephant.UnityLibrary.Localizations**. Add the **LocalizationManager** to a **GameObject** in your startup scene.
- The LocalizationManager will load the localizations before the scene even loads. Feel free to use it in your **Awake()** methods.
- Your localization CSV files should be put in the **Resources** folder. Example: **Assets/Resources/Localizations/Localization_en.csv**

## Example usage

```c#
// Set language to English. By default the languageKey must match the suffix of your CSV file.
// The example below by default matches file: "Assets/Resources/Localizations/Localization_en.csv".
LocalizationManager.Instance.SetLanguage("en");

// Log the current language key.
Debug.Log(LocalizationManager.Instance.CurrentLanguageKey);

// Translate the "music" keyword.
string musicText = LocalizationManager.Instance.Localize("music");

// Optionally subscribe to OnLanguageChanged.
LocalizationManager.Instance.OnLanguageChanged += RefreshLocalization;
private void RefreshLocalization()
{
	// Put your localization refresh work here.
}
```

## CSV content example

Note that the CSV header row is required.

```
Key,Translation
musicVolume,Musiklautstärke
soundVolume,Soundlautstärke
```

## Use different directories and/or filenames

You may inherit from **LocalizationManager** and override **BuildDirectoriesAndFilename()**.

```c#
protected override string BuildDirectoriesAndFilename(string languageKey)
{
	// return $"Localizations/Localization_{languageKey}"; // Default implementation.
	// Your implementation here.
	// The return value must NOT include the "Resources/" folder nor anything before that.
	// The return value must NOT include the file extension.
}

// You may also override the default language key (defaults to "en" by default).
protected override string DefaultLanguageKey() => "your_language_key_here";
```

# String Operations

```c#
StringOperations.CapitalizeFirstChar(string stringToCapitalize)
StringOperations.CapitalizeFirstCharNullable(string? stringToCapitalize)
StringOperations.ConvertToBool(string? value) // Accepts: "true", "True", "false", "False", 1, 0, "", <null>
StringOperations.EncloseByIfNotAlready(string value, char encloser)
StringOperations.Join(char separatorChar, params string?[] stringsToCombine)
StringOperations.JoinWithLeading(char separatorChar, params string?[] stringsToCombine)
StringOperations.JoinWithTrailing(char separatorChar, params string?[] stringsToCombine)
StringOperations.JoinWithLeadingAndTrailing(char separatorChar, params string?[] stringsToCombine)
StringOperations.RemoveSubstringFromString(string source, string substringToRemove)
StringOperations.RemoveSubstringsFromString(string source, IEnumerable<string> substringsToRemove)
StringOperations.SplitByNewLine(string value, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
StringOperations.ToTitleCase(string stringToTitleCase)
StringOperations.ToTitleCaseNullable(string? stringToTitleCase)
```

# FAQ

### Q: I don't see any logs in the console.

### A: Ensure that the following settings have been set somewhere prior to logging:

```c#
// Anywhere prior to logging:
// Allows all console log types globally.
Debug.unityLogger.filterLogType = LogType.Log;
// Allows console logging globally.
Debug.unityLogger.logEnabled = true;
    
// IConsoleLogger (or the static version if you use it):
// Allows all log types.
FilterLogType = LogType.Log;
// Allows logging.
LogEnabled = true;
```

# For developers whom want to edit this project directly

If you need updated versions of the **UnityEditor.dll** and **UnityEngine.dll** files for when editing this project directly, they can be found in the following locations:

- Windows:<Unity Installation Path>\Editor\Data\Managed\
  - Example: C:\Program Files\Unity\Hub\Editor\2022.3.7f1\Editor\Data\Managed\UnityEditor.dll

- MacOS: /Applications/Unity/Unity.app/Contents/Managed/

# Version history

There isn't any. I don't keep a version history for this project.
