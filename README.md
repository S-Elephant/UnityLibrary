# Unity library

Written for Unity Editor version **2022.3.7f1 (LTS)**. Note that most, if not all, earlier versions also seem to work but it's not recommend using this library in Unity Editor versions that are really old.

# CoroutineStarter

Allows you to start coroutines from non-monobehaviour code and can optionally also manage and track coroutines because Unity does not support a CancellationToken.

## Example

```c#
private IEnumerator GrowAllPikachus(int someValue, Action? SomeOptionalCallback)
{
	yield return new WaitForSeconds(3); // Do something that takes time.
    SomeOptionalCallback?.Invoke();
}

private void SomeOptionalCallback()
{
    // Do something after your coroutine finished.
}

private void Awake()
{
	CoroutineStarter.Instance.StartCoroutineManaged(GrowAllPikachus(2, SomeOptionalCallback), "PokemonApi", "Growing");
}

private void Start()
{
	CoroutineStarter.Instance.StopCoroutinesManaged("PokemonApi"); // Stops all coroutines in the "PokemonApi" main category.
	CoroutineStarter.Instance.StopCoroutinesManaged("PokemonApi", "Growing"); // Stops all coroutines in the "PokemonApi" main-category that also are located in the "Growing sub-category".
}
```

## Methods, properties and functions

```c#
CoroutineStarter.UncategorizedValue // Used as a default value when no main- or sub-category is specified.
CoroutineStarter.Instance // Access the CoroutineStarter its singleton instance.    
CoroutineStarter.StopAllOnUnloadScene // If true, will stop and untrack all coroutiness when the scene is being unloaded. Defaults to false.
CoroutineStarter.MainCategoriesToStopOnUnloadScene // All main-categories to stop and untrack when the scene is being unloaded. Defaults to none.

public Coroutine StartCoroutineManaged(IEnumerator routine, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, params string[] tags)

public Coroutine StartCoroutineNamedManaged(IEnumerator routine, string? coroutineName = null, string mainCategory = UncategorizedValue, string subCategory = UncategorizedValue, bool stopExisting = true, params string[] tags)

public int StopAllManagedCoroutines()

public bool StopCoroutineManaged(string coroutineName)

public List<string> StopCoroutinesManaged(string? mainCategory = null, string? subCategory = null, List<string>? tags = null)
```



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



## FAQ

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
