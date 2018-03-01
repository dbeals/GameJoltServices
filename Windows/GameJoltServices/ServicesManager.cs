using System;
using System.IO;
using GameJolt.Async;
using GameJoltServices.Requests;

namespace GameJoltServices
{
	public sealed class ServicesManager
	{
		public string GameId { get; private set; }

		public string PrivateKey { get; private set; }

		internal bool IsUsingDevData { get; }

		public ServicesManager(string gameId, string privateKey)
		{
			if (string.IsNullOrWhiteSpace(gameId))
				throw new ArgumentException("You must supply a valid game ID string.", "gameId");
			if (string.IsNullOrWhiteSpace(privateKey))
				throw new ArgumentException("You must supply a valid private key string.", "privateKey");

			if (gameId.ToLower() == "devgameid" && privateKey.ToLower() == "devprivatekey")
				IsUsingDevData = true;

			GameId = gameId;
			PrivateKey = privateKey;
		}

		public IAsyncResult BeginLogInUser(string username, string userToken, UserIndex userIndex = UserIndex.Any, AsyncCallback callback = null, object asyncState = null)
		{
			var request = new LogInRequest(this, username, userToken, userIndex);
			return request.Begin(callback, asyncState);
		}

		public LoggedInUser EndLogInUser(IAsyncResult result)
		{
			var joltResult = (AsyncResult<LoggedInUser>) result;
			var request = (LogInRequest) joltResult.CoreData;
			return request.End(result);
		}

		public LoggedInUser LogInUser(string username, string userToken, UserIndex userIndex = UserIndex.Any)
		{
			var request = new LogInRequest(this, username, userToken, userIndex);
			return request.Process(null);
		}

		public IAsyncResult LogInUser(string username, string userToken, UserIndex userIndex, Action<LoggedInUser> callback)
		{
			return BeginLogInUser(username, userToken, userIndex, result => callback(EndLogInUser(result)));
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