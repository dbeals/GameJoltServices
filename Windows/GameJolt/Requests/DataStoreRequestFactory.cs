using System;
using Newtonsoft.Json;

namespace GameJolt.Requests
{
	internal static class DataStoreRequestFactory
	{
		[JsonObject]
		internal sealed class DataStoreKeysResult : IJsonResult<DataStore[]>
		{
			[JsonProperty("response")]
			public DataStoreKeysResponse Response { get; set; }

			IJsonResponse<DataStore[]> IJsonResult<DataStore[]>.Response => Response;
		}

		[JsonObject]
		internal sealed class DataStoreKeysResponse : IJsonResponse<DataStore[]>
		{
			[JsonProperty("success")]
			public string WasSuccessful { get; set; }

			[JsonProperty("message")]
			public string Message { get; set; }

			[JsonProperty("keys")]
			public DataStore[] Keys { get; set; }

			DataStore[] IJsonResponse<DataStore[]>.ResultSet => Keys;
		}

		public static JsonRequest<DataStore[], DataStoreKeysResult> CreateGetRequest(string gameId, string publicKey, string username, string userToken, DataStoreType type)
		{
			var output = new JsonRequest<DataStore[], DataStoreKeysResult>(gameId, publicKey, Constants.DataStoreUrls.GetKeysUrl);
			if (type != DataStoreType.User)
				return output;

			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentException("You must specify a username when retrieving user keys.");
			if (string.IsNullOrWhiteSpace(userToken))
				throw new ArgumentException("You must specify a user token when retrieving user keys.");
			output.Parameters.Add(RequestParameter.CreateUsername(username));
			output.Parameters.Add(RequestParameter.CreateUserToken(userToken));

			return output;
		}

		public static DumpRequest CreateGetRequest(string gameId, string publicKey, string username, string userToken, string key)
		{
			var output = new DumpRequest(gameId, publicKey, Constants.DataStoreUrls.GetUrl);
			output.Parameters.Add(RequestParameter.CreateKey(key));
			if (string.IsNullOrWhiteSpace(username))
				return output;

			if (string.IsNullOrWhiteSpace(userToken))
				throw new ArgumentException("You must specify a user token when retrieving user keys.");
			output.Parameters.Add(RequestParameter.CreateUsername(username));
			output.Parameters.Add(RequestParameter.CreateUserToken(userToken));

			return output;
		}

		public static StringRequest CreateSetRequest(string gameId, string publicKey, string username, string userToken, string key, string data)
		{
			var output = new StringRequest(gameId, publicKey, Constants.DataStoreUrls.SetUrl, RequestParameter.CreateKey(key), RequestParameter.CreateData(data));
			if (string.IsNullOrWhiteSpace(username))
				return output;

			if (string.IsNullOrWhiteSpace(userToken))
				throw new ArgumentException("You must specify a user token when retrieving user keys.");
			output.Parameters.Add(RequestParameter.CreateUsername(username));
			output.Parameters.Add(RequestParameter.CreateUserToken(userToken));

			return output;
		}

		public static DumpRequest CreateUpdateRequest(string gameId, string publicKey, string username, string userToken, string key, DataStoreOperation operation, string value)
		{
			var output = new DumpRequest(gameId, publicKey, Constants.DataStoreUrls.UpdateUrl, RequestParameter.CreateKey(key), RequestParameter.CreateOperation(operation), RequestParameter.CreateValue(value));
			if (string.IsNullOrWhiteSpace(username))
				return output;

			if (string.IsNullOrWhiteSpace(userToken))
				throw new ArgumentException("You must specify a user token when retrieving user keys.");
			output.Parameters.Add(RequestParameter.CreateUsername(username));
			output.Parameters.Add(RequestParameter.CreateUserToken(userToken));

			return output;
		}

		public static StringRequest CreateRemoveRequest(string gameId, string publicKey, string username, string userToken, string key)
		{
			var output = new StringRequest(gameId, publicKey, Constants.DataStoreUrls.RemoveUrl, RequestParameter.CreateKey(key));
			if (string.IsNullOrWhiteSpace(username))
				return output;

			if (string.IsNullOrWhiteSpace(userToken))
				throw new ArgumentException("You must specify a user token when retrieving user keys.");
			output.Parameters.Add(RequestParameter.CreateUsername(username));
			output.Parameters.Add(RequestParameter.CreateUserToken(userToken));

			return output;
		}
	}
}