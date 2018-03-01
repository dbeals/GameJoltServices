using System;
using GameJolt.Async;
using GameJolt.Requests;
using Newtonsoft.Json;

namespace GameJolt
{
	[JsonObject]
	public sealed class Trophy
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("difficulty")]
		public string Difficulty { get; set; }

		[JsonProperty("image_url")]
		public string Image { get; set; }

		[JsonProperty("achieved")]
		public string Achieved { get; set; }

		public static IAsyncResult BeginGetById(string gameId, string privateKey, string username, string userToken, string trophyId, AsyncCallback callback = null, object asyncState = null)
		{
			var request = TrophyRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, trophyId);
			return request.Begin(callback, asyncState);
		}

		public static Trophy EndGetById(IAsyncResult result)
		{
			var joltResult = (AsyncResult<Trophy[]>) result;
			var request = (JsonRequest<Trophy[], TrophyRequestFactory.TrophyResult>) joltResult.CoreData;
			var output = request.End(result);
			return output[0];
		}

		public static Trophy GetById(string gameId, string privateKey, string username, string userToken, string trophyId)
		{
			var request = TrophyRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, trophyId);
			return request.Process(null)[0];
		}

		public static IAsyncResult BeginGetAll(string gameId, string privateKey, string username, string userToken, TrophyFilter filter = TrophyFilter.All, AsyncCallback callback = null, object asyncState = null)
		{
			var request = TrophyRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, filter);
			return request.Begin(callback, asyncState);
		}

		public static Trophy[] EndGetAll(IAsyncResult result)
		{
			var joltResult = (AsyncResult<Trophy[]>) result;
			var request = (JsonRequest<Trophy[], TrophyRequestFactory.TrophyResult>) joltResult.CoreData;
			return request.End(result);
		}

		public static Trophy[] GetAll(string gameId, string privateKey, string username, string userToken, TrophyFilter filter = TrophyFilter.All)
		{
			var request = TrophyRequestFactory.CreateGetRequest(gameId, privateKey, username, userToken, filter);
			return request.Process(null);
		}

		public static IAsyncResult GetAll(string gameId, string privateKey, string username, string userToken, TrophyFilter filter, Action<Trophy[]> callback)
		{
			return BeginGetAll(gameId, privateKey, username, userToken, filter, result => callback(EndGetAll(result)));
		}

		public static IAsyncResult BeginAddTrophy(string gameId, string privateKey, string username, string userToken, string trophyId, AsyncCallback callback = null, object asyncState = null)
		{
			var request = TrophyRequestFactory.CreateAddAchieveRequest(gameId, privateKey, username, userToken, trophyId);
			return request.Begin(callback, asyncState);
		}

		public static string EndAddTrophy(IAsyncResult result)
		{
			var joltResult = (AsyncResult<string>) result;
			var request = (StringRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static string AddTrophy(string gameId, string privateKey, string username, string userToken, string trophyId)
		{
			var request = TrophyRequestFactory.CreateAddAchieveRequest(gameId, privateKey, username, userToken, trophyId);
			return request.Process(null);
		}

		public static IAsyncResult AddTrophy(string gameId, string privateKey, string username, string userToken, string trophyId, Action<string> callback)
		{
			return BeginAddTrophy(gameId, privateKey, username, userToken, trophyId, result => callback(EndAddTrophy(result)));
		}
	}
}