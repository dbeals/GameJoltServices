using System;
using GameJolt.Async;
using GameJolt.Requests;
using Newtonsoft.Json;

namespace GameJolt
{
	[JsonObject]
	public sealed class HighScoreTable
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("primary")]
		public string Primary { get; set; }

		public static IAsyncResult BeginGetTables(string gameId, string privateKey, AsyncCallback callback = null, object asyncState = null)
		{
			var request = ScoreRequestFactory.CreateGetTablesRequest(gameId, privateKey);
			return request.Begin(callback, asyncState);
		}

		public static HighScoreTable[] EndGetTables(IAsyncResult result)
		{
			var joltResult = (AsyncResult<HighScoreTable[]>) result;
			var request = (JsonRequest<HighScoreTable[], ScoreRequestFactory.HighScoreTableRequestResult>) joltResult.CoreData;
			return request.End(result);
		}

		public static HighScoreTable[] GetTables(string gameId, string privateKey)
		{
			var request = ScoreRequestFactory.CreateGetTablesRequest(gameId, privateKey);
			return request.Process(null);
		}

		public static IAsyncResult GetTables(string gameId, string privateKey, Action<HighScoreTable[]> callback)
		{
			return BeginGetTables(gameId, privateKey, result => callback(EndGetTables(result)));
		}
	}
}