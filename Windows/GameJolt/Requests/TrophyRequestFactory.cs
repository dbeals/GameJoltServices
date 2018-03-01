using Newtonsoft.Json;

namespace GameJolt.Requests
{
	internal static class TrophyRequestFactory
	{
		[JsonObject]
		internal sealed class TrophyResult : IJsonResult<Trophy[]>
		{
			[JsonProperty("response")]
			public TrophyResponse Response { get; set; }

			IJsonResponse<Trophy[]> IJsonResult<Trophy[]>.Response => Response;
		}

		[JsonObject]
		internal sealed class TrophyResponse : IJsonResponse<Trophy[]>
		{
			[JsonProperty("success")]
			public string WasSuccessful { get; set; }

			[JsonProperty("message")]
			public string Message { get; set; }

			[JsonProperty("trophies")]
			public Trophy[] Trophies { get; set; }

			Trophy[] IJsonResponse<Trophy[]>.ResultSet => Trophies;
		}

		public static JsonRequest<Trophy[], TrophyResult> CreateGetRequest(string gameId, string publicKey, string username, string userToken, TrophyFilter filter)
		{
			var request = new JsonRequest<Trophy[], TrophyResult>(gameId, publicKey, Constants.TrophyUrls.GetUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken));
			switch (filter)
			{
				case TrophyFilter.Achieved:
				{
					request.Parameters.Add("achieved", true);
					break;
				}

				case TrophyFilter.Unachieved:
				{
					request.Parameters.Add("unachieved", false);
					break;
				}
			}
			return request;
		}

		public static JsonRequest<Trophy[], TrophyResult> CreateGetRequest(string gameId, string privateKey, string username, string userToken, string trophyId)
		{
			return new JsonRequest<Trophy[], TrophyResult>(gameId, privateKey, Constants.TrophyUrls.GetUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken), RequestParameter.CreateTrophyId(trophyId));
		}

		public static StringRequest CreateAddAchieveRequest(string gameId, string publicKey, string username, string userToken, string trophyId)
		{
			return new StringRequest(gameId, publicKey, Constants.TrophyUrls.AddAchieveUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken), RequestParameter.CreateTrophyId(trophyId));
		}
	}
}