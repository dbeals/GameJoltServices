using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text;
using GameJolt;
using GameJolt.Async;

namespace GameJoltServices.Saves
{
	/// <summary>
	/// </summary>
	public sealed class GameSaveService : ServiceBase
	{
		public string CacheFolder { get; set; }

		public GameSaveService(string cacheFolder, ServicesManager manager)
			: base(manager)
		{
			CacheFolder = cacheFolder;
		}

		public IAsyncResult BeginGetSaves(LoggedInUser user)
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, user.Username, user.UserToken, "GameSaves", null, user);
		}

		public GameSave[] EndGetSaves(IAsyncResult result)
		{
			var dataStore = DataStore.EndGetByKey(result);
			return dataStore == null ? new GameSave[0] : GetSavesFromData(dataStore.Data, dataStore.Username, dataStore.UserToken);
		}

		public GameSave[] GetSaves(LoggedInUser user)
		{
			var dataStore = DataStore.GetByKey(Manager.GameId, Manager.PrivateKey, user.Username, user.UserToken, "GameSaves");
			return dataStore == null ? new GameSave[0] : GetSavesFromData(dataStore.Data, dataStore.Username, dataStore.UserToken);
		}

		public IAsyncResult GetSaves(LoggedInUser user, Action<GameSave[]> callback)
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, user.Username, user.UserToken, "GameSaves", result => callback(EndGetSaves(result)));
		}

		public IAsyncResult BeginDownloadSave(GameSave gameSave)
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, gameSave.Username, gameSave.UserToken, gameSave.Location, null, gameSave);
		}

		public bool EndDownloadSave(IAsyncResult result)
		{
			var dataStore = DataStore.EndGetByKey(result);
			var gameSave = (GameSave) result.AsyncState;
			var requestResult = CacheSave(dataStore.Username, dataStore.UserToken, dataStore.Key, gameSave.DescriptionData, dataStore.Data);
			var wasSuccessful = !string.IsNullOrWhiteSpace(requestResult);

			if (wasSuccessful)
			{
				gameSave.Location = requestResult;
				gameSave.NeedsToBeDownloaded = false;
			}

			return wasSuccessful;
		}

		public bool DownloadSave(GameSave gameSave)
		{
			var dataStore = DataStore.GetByKey(Manager.GameId, Manager.PrivateKey, gameSave.Username, gameSave.UserToken, gameSave.Location);
			var requestResult = CacheSave(dataStore.Username, dataStore.UserToken, dataStore.Key, gameSave.DescriptionData, dataStore.Data);
			var wasSuccessful = !string.IsNullOrWhiteSpace(requestResult);

			if (wasSuccessful)
			{
				gameSave.Location = requestResult;
				gameSave.NeedsToBeDownloaded = false;
			}

			return wasSuccessful;
		}

		public IAsyncResult DownloadSave(GameSave gameSave, Action<bool> callback)
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, gameSave.Username, gameSave.UserToken, gameSave.Location, result => callback(EndDownloadSave(result)), gameSave);
		}

		public IAsyncResult BeginUploadSave(GameSave gameSave)
		{
			var request = new UploadSaveRequest(this, gameSave);
			return request.Begin(null, null);
		}

		public bool EndUploadSave(IAsyncResult result)
		{
			var joltResult = (AsyncResult<LoggedInUser>) result;
			var request = (UploadSaveRequest) joltResult.CoreData;
			return request.End(result);
		}

		public bool UploadSave(GameSave gameSave)
		{
			var request = new UploadSaveRequest(this, gameSave);
			return request.Process(null);
		}

		public IAsyncResult UploadSave(GameSave gameSave, Action<bool> callback)
		{
			var request = new UploadSaveRequest(this, gameSave);
			return request.Begin(result => callback(EndUploadSave(result)), null);
		}

		public bool CacheSave(GameSave gameSave)
		{
			return !string.IsNullOrWhiteSpace(CacheSave(gameSave.Username, gameSave.UserToken, gameSave.Location, gameSave.DescriptionData, gameSave.SaveData));
		}

		internal IAsyncResult UpdateSaves(string username, string userToken, params GameSave[] newSaves)
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, username, userToken, "GameSaves", result =>
			{
				DataStore dataStore = null;
				try
				{
					dataStore = DataStore.EndGetByKey(result);
				}
				catch
				{
					// the key does not exist.
				}

				var savesDictionary = new Dictionary<string, GameSave>();

				if (dataStore != null)
				{
					var saves = GetSavesFromData(dataStore.Data, dataStore.Username, dataStore.UserToken);
					foreach (var save in saves)
						savesDictionary[save.IsCached ? Path.GetFileNameWithoutExtension(save.Location) : save.Location] = save;
				}

				foreach (var save in newSaves)
					savesDictionary[save.IsCached ? Path.GetFileNameWithoutExtension(save.Location) : save.Location] = save;

				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						foreach (var savePair in savesDictionary)
						{
							var save = savePair.Value;
							writer.Write(save.Location);
							writer.Write(save.Name);
							writer.Write(save.DateSaved.Ticks);
							writer.Write(save.DescriptionData);
						}

						DataStore.BeginSetByKey(Manager.GameId, Manager.PrivateKey, username, userToken, "GameSaves", Convert.ToBase64String(stream.ToArray()).Replace("+", "%2b"));
					}
				}
			});
		}

		private string GetCacheFilePath(string location)
		{
			if (Path.IsPathRooted(location))
				return location;
			return Path.Combine(CacheFolder, "Saves", location + ".cgs");
		}

		private string CacheTest(string location, DateTime dateTimeSaved)
		{
			var fullFilePath = GetCacheFilePath(location);
			if (File.Exists(fullFilePath))
			{
				var fileInfo = new FileInfo(fullFilePath);
				if (fileInfo.LastWriteTime.CompareTo(dateTimeSaved) >= 0)
					return fullFilePath;
			}

			return location;
		}

		private GameSave[] GetSavesFromData(string data, string username, string userToken)
		{
			data = data.Replace("%2b", "+");
			var output = new List<GameSave>();
			using (var stream = new MemoryStream(Convert.FromBase64String(data)))
			{
				using (var reader = new BinaryReader(stream))
				{
					var location = reader.ReadString();
					var name = reader.ReadString();
					var dateSaved = new DateTime(reader.ReadInt64());
					var descriptionData = reader.ReadString();
					location = CacheTest(location, dateSaved);

					output.Add(new GameSave
					{
						Username = username,
						UserToken = userToken,
						Location = location,
						Name = name,
						DateSaved = dateSaved,
						DescriptionData = descriptionData,
						NeedsToBeDownloaded = !Path.IsPathRooted(location)
					});
				}
			}

			return output.ToArray();
		}

		private string CacheSave(string username, string userToken, string dataStoreKey, string descriptionData, string data = null)
		{
			try
			{
				if (data == null)
				{
					var dataStore = DataStore.GetByKey(Manager.GameId, Manager.PrivateKey, username, userToken, dataStoreKey);
					data = dataStore.Data;
				}

				var filePath = GetCacheFilePath(dataStoreKey);
				Helper.EnsureDirectoryExists(filePath);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					using (var writer = new BinaryWriter(stream))
					{
						writer.Write("GJS1");
						writer.Write(username);
						writer.Write(descriptionData);
						writer.Write(data); // Save data is stored as a base-64 string containing a base-64 string for the description and a base-64 string for the data.
					}
				}

				return filePath;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}