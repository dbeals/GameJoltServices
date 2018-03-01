using System;
using Newtonsoft.Json;

namespace GameJolt.Requests
{
	internal static class ScoreRequestFactory
	{
		[JsonObject]
		internal sealed class ScoreRequestResult : IJsonResult<Score[]>
		{
			[JsonProperty("response")]
			public ScoreRequestResponse Response { get; set; }

			IJsonResponse<Score[]> IJsonResult<Score[]>.Response => Response;
		}

		[JsonObject]
		internal sealed class ScoreRequestResponse : IJsonResponse<Score[]>
		{
			public string WasSuccessful { get; set; }

			public string Message { get; set; }

			[JsonProperty("scores")]
			public Score[] Scores { get; set; }

			Score[] IJsonResponse<Score[]>.ResultSet => Scores;
		}

		[JsonObject]
		internal sealed class HighScoreTableRequestResult : IJsonResult<HighScoreTable[]>
		{
			[JsonProperty("response")]
			public HighScoreTableRequestResponse Response { get; set; }

			IJsonResponse<HighScoreTable[]> IJsonResult<HighScoreTable[]>.Response => Response;
		}

		[JsonObject]
		internal sealed class HighScoreTableRequestResponse : IJsonResponse<HighScoreTable[]>
		{
			[JsonProperty("success")]
			public string WasSuccessful { get; set; }

			[JsonProperty("message")]
			public string Message { get; set; }

			[JsonProperty("tables")]
			public HighScoreTable[] Tables { get; set; }

			HighScoreTable[] IJsonResponse<HighScoreTable[]>.ResultSet => Tables;
		}

		public static JsonRequest<Score[], ScoreRequestResult> CreateGetRequest(string gameId, string privateKey, string username, string userToken, int limit, string tableId)
		{
			var output = new JsonRequest<Score[], ScoreRequestResult>(gameId, privateKey, Constants.ScoreUrls.GetUrl);
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(userToken))
			{
				if (!string.IsNullOrWhiteSpace(username))
				{
					Logging.LogError("If you do not want user specific scores you cannot supply a username or a user token.");
					throw new Exception("User token is null, but username is not.");
				}

				if (!string.IsNullOrWhiteSpace(userToken))
				{
					Logging.LogError("If you do not want user specific scores you cannot supply a username or a user token.");
					throw new Exception("Username is null, but user token is not.");
				}
			}
			else
			{
				output.Parameters.Add(RequestParameter.CreateUsername(username));
				output.Parameters.Add(RequestParameter.CreateUserToken(userToken));
			}

			if (limit > 0)
				output.Parameters.Add(RequestParameter.CreateLimit(limit));

			if (!string.IsNullOrWhiteSpace(tableId))
				output.Parameters.Add(RequestParameter.CreateTableId(tableId));

			return output;
		}

		public static StringRequest CreateAddRequest(string gameId, string privateKey, string score, int sort, string username, string userToken, string guest, string extraData, string tableId)
		{
			var output = new StringRequest(gameId, privateKey, Constants.ScoreUrls.AddUrl, RequestParameter.CreateScore(score), RequestParameter.CreateSort(sort));

			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(userToken))
			{
				if (!string.IsNullOrWhiteSpace(username))
				{
					Logging.LogError("If you do not want user specific scores you cannot supply a username or a user token.");
					throw new Exception("User token is null, but username is not.");
				}

				if (!string.IsNullOrWhiteSpace(userToken))
				{
					Logging.LogError("If you do not want user specific scores you cannot supply a username or a user token.");
					throw new Exception("Username is null, but user token is not.");
				}

				output.Parameters.Add(RequestParameter.CreateGuest(guest));
			}
			else
			{
				output.Parameters.Add(RequestParameter.CreateUsername(username));
				output.Parameters.Add(RequestParameter.CreateUserToken(userToken));
			}

			if (!string.IsNullOrWhiteSpace(extraData))
				output.Parameters.Add(RequestParameter.CreateExtraData(extraData));

			if (!string.IsNullOrWhiteSpace(tableId))
				output.Parameters.Add(RequestParameter.CreateTableId(tableId));

			return output;
		}

		public static JsonRequest<HighScoreTable[], HighScoreTableRequestResult> CreateGetTablesRequest(string gameId, string privateKey)
		{
			return new JsonRequest<HighScoreTable[], HighScoreTableRequestResult>(gameId, privateKey, Constants.ScoreUrls.TablesUrl);
		}
	}
}