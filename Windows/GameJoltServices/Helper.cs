using System;
using System.IO;
using System.Linq;
using GameJolt;

namespace GameJoltServices
{
	public static class Helper
	{
		public static bool GameDataStoreExists(string gameId, string privateKey, string dataStoreKey)
		{
			var keys = DataStore.GetKeys(gameId, privateKey, null, null, DataStoreType.Game);
			if (keys == null || keys.Length == 0)
				return false;

			return keys.Contains(dataStoreKey);
		}

		public static bool UserDataStoreExists(string gameId, string privateKey, string username, string userToken, string dataStoreKey)
		{
			var keys = DataStore.GetKeys(gameId, privateKey, username, userToken, DataStoreType.User);
			if (keys == null || keys.Length == 0)
				return false;

			return keys.Contains(dataStoreKey);
		}

		public static bool EnsureDirectoryExists(string fullPath)
		{
			if (Path.HasExtension(fullPath))
				fullPath = Path.GetDirectoryName(fullPath);

			if (Directory.Exists(fullPath))
				return true;
			Directory.CreateDirectory(fullPath);
			return false;
		}

		public static string GetDefaultCacheFolder(string gameFolder)
		{
#if PLATFORM_PC
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", gameFolder);
#elif PLATFORM_ANDROID
				return System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, gameFolder);
#endif
		}
	}
}