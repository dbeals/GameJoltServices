using System;
using System.Linq;
using System.Threading.Tasks;
using GameJolt.Async;
using GameJolt.Requests;
using Newtonsoft.Json;

namespace GameJolt
{
	[JsonObject]
	public sealed class DataStore
	{
		[JsonIgnore]
		public DataStoreType Type { get; set; }

		[JsonProperty("key")]
		public string Key { get; set; }

		public string Data { get; set; }
		public string Username { get; set; }
		public string UserToken { get; set; }

		public DataStore() { }

		public DataStore(DataStoreType type, string key, string data, string username, string userToken)
		{
			Type = type;
			Key = key;
			Data = data;
			Username = username;
			UserToken = userToken;
		}

		public override string ToString()
		{
			return $"Type={Type},Key={Key},Data={Data}";
		}

		public static IAsyncResult BeginGetKeys(string gameId, string privateKey, string username, string userToken, DataStoreType type, AsyncCallback callback = null, object asyncState = null)
		{
			var request = DataStoreRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, type);
			return request.Begin(callback, asyncState);
		}

		public static string[] EndGetKeys(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string[]>) result;
			var request = (JsonRequest<DataStore[], DataStoreRequestFactory.DataStoreKeysResult>) joltResult.CoreData;
			var keys = request.End(result);
			if (keys == null)
				return null;
			return (from key in keys
				select key.Key).ToArray();
		}

		public static string[] GetKeys(string gameId, string privateKey, string username, string userToken, DataStoreType type)
		{
			var request = DataStoreRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, type);
			var keys = request.Process(null);

			if (keys == null)
				return null;
			return (from key in keys
				select key.Key).ToArray();
		}

		public static IAsyncResult GetKeys(string gameId, string privateKey, string username, string userToken, DataStoreType type, Action<string[]> callback)
		{
			return BeginGetKeys(gameId, privateKey, username, userToken, type, result => callback(EndGetKeys(result)));
		}

		public static Task<string[]> GetKeysAsync(string gameId, string privateKey, string username, string userToken, DataStoreType type)
		{
			return AsyncHelper.AsyncCall(callback => BeginGetKeys(gameId, privateKey, username, userToken, type, callback), EndGetKeys);
		}

		public static IAsyncResult BeginGetByKey(string gameId, string privateKey, string username, string userToken, string key, AsyncCallback callback = null, object asyncState = null)
		{
			var request = DataStoreRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, key);
			return request.Begin(callback, asyncState);
		}

		public static DataStore EndGetByKey(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (DumpRequest) joltResult.CoreData;
			var data = request.End(result);

			if (!request.WasSuccessful)
				throw new Exception(data);

			var isGame = request.Parameters.Contains(RequestParameter.UsernameKey);
			return new DataStore(isGame ? DataStoreType.Game : DataStoreType.User, (string) request.Parameters[RequestParameter.KeyKey], data, (string) request.Parameters[RequestParameter.UsernameKey], (string) request.Parameters[RequestParameter.UserTokenKey]);
		}

		public static DataStore GetByKey(string gameId, string privateKey, string username, string userToken, string key)
		{
			var request = DataStoreRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, key);
			var data = request.Process(null);
			if (!request.WasSuccessful)
			{
				Logging.LogWarning(data);
				return null;
			}

			var isGame = string.IsNullOrWhiteSpace(username);
			return new DataStore(isGame ? DataStoreType.Game : DataStoreType.User, key, data, username, userToken);
		}

		public static IAsyncResult GetByKey(string gameId, string privateKey, string username, string userToken, string key, Action<DataStore> callback)
		{
			return BeginGetByKey(gameId, privateKey, username, userToken, key, result => callback(EndGetByKey(result)));
		}

		public static Task<DataStore> GetByKeyAsync(string gameId, string privateKey, string username, string userToken, string key)
		{
			return AsyncHelper.AsyncCall(callback => BeginGetByKey(gameId, privateKey, username, userToken, key, callback), EndGetByKey);
		}

		public static IAsyncResult BeginSetByKey(string gameId, string privateKey, string username, string userToken, string key, string data, AsyncCallback callback = null, object asyncState = null)
		{
			var request = DataStoreRequestFactory.CreateSetRequest(gameId, privateKey, username, userToken, key, data);
			return request.Begin(callback, asyncState);
		}

		public static string EndSetByKey(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static string SetByKey(string gameId, string privateKey, string username, string userToken, string key, string data)
		{
			var request = DataStoreRequestFactory.CreateSetRequest(gameId, privateKey, username, userToken, key, data);
			return request.Process(null);
		}

		public static IAsyncResult SetByKey(string gameId, string privateKey, string username, string userToken, string key, string data, Action<string> callback)
		{
			return BeginSetByKey(gameId, privateKey, username, userToken, key, data, result => callback(EndSetByKey(result)));
		}

		public static Task<string> SetByKeyAsync(string gameId, string privateKey, string username, string userToken, string key, string data)
		{
			return AsyncHelper.AsyncCall(callback => BeginSetByKey(gameId, privateKey, username, userToken, key, data, callback), EndSetByKey);
		}

		public static IAsyncResult BeginUpdateByKey(string gameId, string privateKey, string username, string userToken, string key, DataStoreOperation operation, string value, AsyncCallback callback = null, object asyncState = null)
		{
			var request = DataStoreRequestFactory.CreateUpdateRequest(gameId, privateKey, username, userToken, key, operation, value);
			return request.Begin(callback, asyncState);
		}

		public static DataStore EndUpdateByKey(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (DumpRequest) joltResult.CoreData;
			var data = request.End(result);

			if (!request.WasSuccessful)
				throw new Exception(data);
			var isGame = request.Parameters.Contains(RequestParameter.UsernameKey);
			return new DataStore(isGame ? DataStoreType.Game : DataStoreType.User, (string) request.Parameters[RequestParameter.KeyKey], data, (string) request.Parameters[RequestParameter.UsernameKey], (string) request.Parameters[RequestParameter.UserTokenKey]);
		}

		public static DataStore UpdateByKey(string gameId, string privateKey, string username, string userToken, string key, DataStoreOperation operation, string value)
		{
			var request = DataStoreRequestFactory.CreateUpdateRequest(gameId, privateKey, username, userToken, key, operation, value);
			var data = request.Process(null);
			var isGame = string.IsNullOrWhiteSpace(username);
			return new DataStore(isGame ? DataStoreType.Game : DataStoreType.User, key, data, username, userToken);
		}

		public static IAsyncResult UpdateByKey(string gameId, string privateKey, string username, string userToken, string key, DataStoreOperation operation, string value, Action<DataStore> callback)
		{
			return BeginUpdateByKey(gameId, privateKey, username, userToken, key, operation, value, result => callback(EndUpdateByKey(result)));
		}

		public static Task<DataStore> UpdateByKeyAsync(string gameId, string privateKey, string username, string userToken, string key, DataStoreOperation operation, string value)
		{
			return AsyncHelper.AsyncCall(callback => BeginUpdateByKey(gameId, privateKey, username, userToken, key, operation, value, callback), EndUpdateByKey);
		}

		public static IAsyncResult BeginRemoveByKey(string gameId, string privateKey, string username, string userToken, string key, AsyncCallback callback = null, object asyncState = null)
		{
			var request = DataStoreRequestFactory.CreateRemoveRequest(gameId, privateKey, username, userToken, key);
			return request.Begin(callback, asyncState);
		}

		public static string EndRemoveByKey(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static string RemoveByKey(string gameId, string privateKey, string username, string userToken, string key)
		{
			var request = DataStoreRequestFactory.CreateRemoveRequest(gameId, privateKey, username, userToken, key);
			return request.Process(null);
		}

		public static IAsyncResult RemoveByKey(string gameId, string privateKey, string username, string userToken, string key, Action<string> callback)
		{
			return BeginRemoveByKey(gameId, privateKey, username, userToken, key, result => callback(EndRemoveByKey(result)));
		}

		public static Task<string> RemoveByKeyAsync(string gameId, string privateKey, string username, string userToken, string key)
		{
			return AsyncHelper.AsyncCall(callback => BeginRemoveByKey(gameId, privateKey, username, userToken, key, callback), EndRemoveByKey);
		}

		public static bool GameDataStoreExists(string gameId, string privateKey, string dataStoreKey)
		{
			var keys = GetKeys(gameId, privateKey, null, null, DataStoreType.Game);
			if (keys == null || keys.Length == 0)
				return false;

			return keys.Contains(dataStoreKey);
		}

		public static bool UserDataStoreExists(string gameId, string privateKey, string username, string userToken, string dataStoreKey)
		{
			var keys = GetKeys(gameId, privateKey, username, userToken, DataStoreType.User);
			if (keys == null || keys.Length == 0)
				return false;

			return keys.Contains(dataStoreKey);
		}
	}
}