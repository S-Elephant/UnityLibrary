using UnityEngine;

namespace Elephant.UnityLibrary.Networks
{
	/// <summary>
	/// Provides utilities for checking network connectivity status.
	/// </summary>
	public static class NetworkStatus
	{
#if UNITY_EDITOR
        /// <summary>
        /// Editor-only override to simulate no internet connection for testing purposes.
        /// </summary>
        public static bool TestOverrideInternetConnectionToUnavailable = false;
#endif

		/// <summary>
		/// Checks if the device has an active internet connection.
		/// </summary>
		/// <returns>
		/// True if internet is reachable (either via mobile data or WiFi),
		/// false if there's no connection or if TestOverrideInternetConnectionToUnavailable is true inside of Unity Editor.
		/// </returns>
		public static bool HasReachableInternetConnection()
		{
#if UNITY_EDITOR
            if (TestOverrideInternetConnectionToUnavailable)
                return false;
#endif
			return Application.internetReachability != NetworkReachability.NotReachable;
		}
	}
}
