using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly.PersistentData
{
	/// <summary>
	/// Open resources folder menu option.
	/// </summary>
	public class OpenResourcesFolderEditor
	{
		/// <summary>
		/// The relative path to the Resources folder.
		/// </summary>
		private const string ResourcesFolder = "Resources";

		/// <summary>
		/// Menu item in Unity Editor to open the specified Resources subfolder in Windows Explorer.
		/// </summary>
		[MenuItem(SettingsEditor.MenuPrefix + "Open Resources folder")]
		public static void OpenFolder()
		{
			string fullPath = System.IO.Path.Combine(Application.dataPath, ResourcesFolder);

#if UNITY_EDITOR_WIN
			OpenInExplorer(fullPath.Replace("/", "\\"));
#elif UNITY_EDITOR_OSX
			OpenInFinder(fullPath.Replace("\\", "/"));
#elif UNITY_EDITOR_LINUX
			OpenInFileManager(fullPath.Replace("\\", "/"));
#endif
		}

#if UNITY_EDITOR_WIN
		/// <summary>
		/// Opens the specified path in Windows Explorer.
		/// </summary>
		/// <param name="path">The full path to the folder to be opened.</param>
		private static void OpenInExplorer(string path)
		{
			if (System.IO.Directory.Exists(path))
				Process.Start("explorer.exe", path);
			else
				UnityEngine.Debug.LogError($"The following path does not exist: \"{path}\". Aborting.");
		}
#elif UNITY_EDITOR_OSX
		/// <summary>
		/// Opens the specified path in macOS Finder.
		/// </summary>
		/// <param name="path">The full path to the folder to be opened.</param>
		private static void OpenInFinder(string path)
		{
			if (System.IO.Directory.Exists(path))
				Process.Start("open", path);
			else
				UnityEngine.Debug.LogError($"The following path does not exist: \"{path}\". Aborting.");
		}
#elif UNITY_EDITOR_LINUX
		/// <summary>
		/// Opens the specified path in the default Linux file manager.
		/// </summary>
		/// <param name="path">The full path to the folder to be opened.</param>
		private static void OpenInFileManager(string path)
		{
			if (System.IO.Directory.Exists(path))
				Process.Start("xdg-open", path);
			else
				UnityEngine.Debug.LogError($"The following path does not exist: \"{path}\". Aborting.");
		}
#endif
	}
}
