using System;
using System.Threading.Tasks;
using GameJolt.Async;
using GameJolt.Requests;
using Newtonsoft.Json;

namespace GameJolt
{
	[JsonObject]
	public class User
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("avatar_url")]
		public string AvatarUrl { get; set; }

		[JsonProperty("signed_up")]
		public string SignedUp { get; set; }

		[JsonProperty("last_logged_in")]
		public string LastLoggedIn { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("developer_name")]
		public string DeveloperName { get; set; }

		[JsonProperty("developer_website")]
		public string DeveloperWebsite { get; set; }

		[JsonProperty("developer_description")]
		public string DeveloperDescription { get; set; }

		public static IAsyncResult BeginGetByUsername(string gameId, string privateKey, string username, AsyncCallback callback = null, object asyncState = null)
		{
			var request = UserRequestFactory.CreateGetRequest(gameId, privateKey, username);
			return request.Begin(callback, asyncState);
		}

		public static User EndGetByUsername(IAsyncResult result)
		{
			var joltResult = (AsyncResult<User[]>) result;
			var request = (JsonRequest<User[], UserRequestFactory.UserResult>) joltResult.CoreData;
			var results = request.End(result);
			if (results == null || results.Length == 0)
				return null;
			return results[0];
		}

		public static User GetByUsername(string gameId, string privateKey, string username)
		{
			var request = UserRequestFactory.CreateGetRequest(gameId, privateKey, username);
			var results = request.Process(null);
			if (results == null || results.Length == 0)
				return null;
			return results[0];
		}

		public static IAsyncResult GetByUsername(string gameId, string privateKey, string username, Action<User> callback)
		{
			return BeginGetByUsername(gameId, privateKey, username, result => callback(EndGetByUsername(result)));
		}

		public static Task<User> GetByUsernameAsync(string gameId, string privateKey, string username)
		{
			return AsyncHelper.AsyncCall(action => { BeginGetByUsername(gameId, privateKey, username, action); }, EndGetByUsername);
		}

		public static IAsyncResult BeginGetByUserIds(string gameId, string privateKey, string[] userIds, AsyncCallback callback = null, object asyncState = null)
		{
			var request = UserRequestFactory.CreateGetRequest(gameId, privateKey, userIds);
			return request.Begin(callback, asyncState);
		}

		public static User[] EndGetByUserIds(IAsyncResult result)
		{
			var joltResult = (AsyncResult<User[]>) result;
			var request = (JsonRequest<User[], UserRequestFactory.UserResult>) joltResult.CoreData;
			return request.End(result);
		}

		public static User[] GetByUserIds(string gameId, string privateKey, params string[] userIds)
		{
			var request = UserRequestFactory.CreateGetRequest(gameId, privateKey, userIds);
			return request.Process(null);
		}

		public static IAsyncResult GetByUserIds(string gameId, string privateKey, string[] userIds, Action<User[]> callback)
		{
			return BeginGetByUserIds(gameId, privateKey, userIds, result => callback(EndGetByUserIds(result)));
		}

		public static Task<User[]> GetByUserIdsAsync(string gameId, string privateKey, string[] userIds)
		{
			return AsyncHelper.AsyncCall(action => { BeginGetByUserIds(gameId, privateKey, userIds, action); }, EndGetByUserIds);
		}

		public static IAsyncResult BeginAuthenticateUser(string gameId, string privateKey, string username, string userToken, AsyncCallback callback = null, object asyncState = null)
		{
			var request = UserRequestFactory.CreateAuthRequest(gameId, privateKey, username, userToken);
			return request.Begin(callback, asyncState);
		}

		public static string EndAuthenticateUser(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static string AuthenticateUser(string gameId, string privateKey, string username, string userToken)
		{
			var request = UserRequestFactory.CreateAuthRequest(gameId, privateKey, username, userToken);
			return request.Process(null);
		}

		public static IAsyncResult AuthenticateUser(string gameId, string privateKey, string username, string userToken, Action<string> callback)
		{
			return BeginAuthenticateUser(gameId, privateKey, username, userToken, result => callback(EndAuthenticateUser(result)));
		}

		public static Task<string> AuthenticateUserAsync(string gameId, string privateKey, string username, string userToken)
		{
			return AsyncHelper.AsyncCall(action => { BeginAuthenticateUser(gameId, privateKey, username, userToken, action); }, EndAuthenticateUser);
		}
	}
}