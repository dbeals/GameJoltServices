using Newtonsoft.Json;

namespace GameJolt.Requests
{
	internal static partial class UserRequestFactory
	{
		[JsonObject]
		internal sealed class UserResult : IJsonResult<User[]>
		{
			[JsonProperty("response")]
			public UserResponse Response { get; set; }

			IJsonResponse<User[]> IJsonResult<User[]>.Response => Response;
		}

		[JsonObject]
		internal sealed class UserResponse : IJsonResponse<User[]>
		{
			[JsonProperty("success")]
			public string WasSuccessful { get; set; }

			[JsonProperty("message")]
			public string Message { get; set; }

			[JsonProperty("users")]
			public User[] Users { get; set; }

			User[] IJsonResponse<User[]>.ResultSet => Users;
		}

		public static JsonRequest<User[], UserResult> CreateGetRequest(string gameId, string publicKey, string username)
		{
			return new JsonRequest<User[], UserResult>(gameId, publicKey, Constants.UserUrls.GetUrl, RequestParameter.CreateUsername(username));
		}

		public static JsonRequest<User[], UserResult> CreateGetRequest(string gameId, string publicKey, params string[] userIDs)
		{
			return new JsonRequest<User[], UserResult>(gameId, publicKey, Constants.UserUrls.GetUrl, RequestParameter.CreateUserId(userIDs));
		}

		public static StringRequest CreateAuthRequest(string gameId, string publicKey, string username, string userToken)
		{
			return new StringRequest(gameId, publicKey, Constants.UserUrls.AuthUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken));
		}
	}
}