using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly.PersistentData
{
	/// <summary>
	/// <see cref="Application.persistentDataPath"/> dev tools.
	/// </summary>
	public class PersistentDataPathEditor : Editor
	{
		/// <summary>
		/// Menu path prefix.
		/// </summary>
		private const string MenuPrefixPath = SettingsEditor.MenuPrefix + "Persistent data path/";

		/// <summary>
		/// <see cref="Application.persistentDataPath"/>.
		/// </summary>
		private static string PersistentDataPath => Application.persistentDataPath;

		/// <summary>
		/// Open persistent data path menu.
		/// </summary>
		[MenuItem(MenuPrefixPath + "Open", false, 5)]
		private static void OpenPersistentDataPathFolder()
		{
			try
			{
				// Open the folder cross-platform.
				Application.OpenURL("file://" + PersistentDataPath);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Error opening persistent data path folder: " + e.Message);
			}
		}

		/// <summary>
		/// Clear persistent data path menu.
		/// </summary>
		[MenuItem(MenuPrefixPath + "Clear", false, 10)]
		private static void ClearPersistentDataPath()
		{
			// Get all info.
			DirectoryInfo rootDirectoryInfo = new DirectoryInfo(PersistentDataPath);
			FileInfo[] fileInfos = rootDirectoryInfo.GetFiles("*", SearchOption.AllDirectories);
			DirectoryInfo[] directoryInfos = DirectoriesSortedByDepthDesc(rootDirectoryInfo);

			// Get file and directory contents in a human readable format.
			string contents = DisplayFilesAndDirectories(rootDirectoryInfo.FullName, fileInfos, directoryInfos);

			if (EditorUtility.DisplayDialog("Clear Persistent Data Path",
					$"Are you sure you want to clear the persistent data path?\nThis action cannot be undone.\n{contents}",
					"Yes, clear", "Cancel"))
			{
				try
				{
					ClearFilesAndDirectories(fileInfos, directoryInfos);
					Debug.Log("Persistent data path cleared successfully.");
				}
				catch (System.Exception e)
				{
					Debug.LogError("Error clearing persistent data path: " + e.Message);
				}
			}
		}

		/// <summary>
		/// Returns files and directories as a string.
		/// </summary>
		private static string DisplayFilesAndDirectories(string rootDirectoryPath, FileInfo[] fileInfos, DirectoryInfo[] directoryInfos)
		{
			const int maxListings = 10;
			StringBuilder resultBuilder = new();
			int listingCount = 0;

			// Display files.
			foreach (FileInfo file in fileInfos)
			{
				if (listingCount >= maxListings)
				{
					resultBuilder.AppendLine("And more..");
					break;
				}

				resultBuilder.AppendLine("File: " + file.FullName.Substring(rootDirectoryPath.Length + 1));
				listingCount++;
			}

			// Display (sub-)directories.
			foreach (DirectoryInfo dir in directoryInfos)
			{
				if (listingCount >= maxListings)
				{
					resultBuilder.AppendLine("And more..");
					break;
				}

				resultBuilder.AppendLine("Directory: " + dir.FullName.Substring(rootDirectoryPath.Length + 1));
				listingCount++;
			}

			return resultBuilder.ToString();
		}

		/// <summary>
		/// Delete all files and folders inside <see cref="Application.persistentDataPath"/>.
		/// </summary>
		private static void ClearFilesAndDirectories(FileInfo[] fileInfos, DirectoryInfo[] directoryInfos)
		{
			foreach (FileInfo fileToDelete in fileInfos)
				fileToDelete.Delete();

			foreach (DirectoryInfo directoryToDelete in directoryInfos)
			{
				if (directoryToDelete.Exists)
					directoryToDelete.Delete(true);
			}
		}

		/// <summary>
		/// Retrieve all directories, sorted by their depth in descending order.
		/// </summary>
		private static DirectoryInfo[] DirectoriesSortedByDepthDesc(DirectoryInfo rootDirectory)
		{
			List<DirectoryInfo> allDirectories = rootDirectory.GetDirectories("*", SearchOption.AllDirectories).ToList();
			allDirectories.Sort((dir1, dir2) => dir2.FullName.Length.CompareTo(dir1.FullName.Length));

			return allDirectories.ToArray();
		}
	}
}
