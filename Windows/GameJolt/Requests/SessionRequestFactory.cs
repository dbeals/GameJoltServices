namespace GameJolt.Requests
{
	internal static class SessionRequestFactory
	{
		public static StringRequest CreateOpenRequest(string gameId, string privateKey, string username, string userToken)
		{
			return new StringRequest(gameId, privateKey, Constants.SessionUrls.OpenUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken));
		}

		public static StringRequest CreatePingRequest(string gameId, string privateKey, string username, string userToken, SessionStatus status)
		{
			return new StringRequest(gameId, privateKey, Constants.SessionUrls.PingUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken), RequestParameter.CreateStatus(status));
		}

		public static StringRequest CreateCloseRequest(string gameId, string privateKey, string username, string userToken)
		{
			return new StringRequest(gameId, privateKey, Constants.SessionUrls.CloseUrl, RequestParameter.CreateUsername(username), RequestParameter.CreateUserToken(userToken));
		}
	}
}