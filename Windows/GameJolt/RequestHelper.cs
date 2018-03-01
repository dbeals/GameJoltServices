using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GameJolt.Async;
using GameJolt.Requests;

namespace GameJolt
{
	public static class RequestHelper
	{
		public static bool WasSuccessful(string result)
		{
			if (string.IsNullOrWhiteSpace(result))
				return false;

			return result.Trim().ToLower() == "success";
		}

		public static IAsyncResult BeginGetImage(string imageUrl, AsyncCallback callback = null, object asyncState = null)
		{
			var request = new ImageRequest(imageUrl);
			return request.Begin(callback, asyncState);
		}

		public static Stream EndGetImage(IAsyncResult result)
		{
			var joltResult = (AsyncResult<Stream>) result;
			var request = (ImageRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static Stream GetImage(string imageUrl)
		{
			var request = new ImageRequest(imageUrl);
			return request.Process(null);
		}

		public static IAsyncResult GetImage(string imageUrl, Action<Stream> callback)
		{
			return BeginGetImage(imageUrl, result => callback(EndGetImage(result)));
		}

		public static string GetStringRequestData(IAsyncResult result)
		{
			var joltResult = (AsyncResultNoResult) result;
			var request = (StringRequest) joltResult.CoreData;
			return request.End(result);
		}

		public static TResult GetJsonRequestData<TResult>(IAsyncResult result)
		{
			var joltResult = (AsyncResultNoResult) result;
			var request = (JsonRequest<TResult, IJsonResult<TResult>>) joltResult.CoreData;
			return request.End(result);
		}

		public static int DifficultyStringToInt(string difficulty)
		{
			switch (difficulty.ToLower())
			{
				case "bronze":
					return 1;

				case "silver":
					return 2;

				case "gold":
					return 3;

				case "platinum":
					return 4;
			}

			throw new NotImplementedException();
		}

		internal static string CreateSignature(string url, string privateKey)
		{
			var stringBuilder = new StringBuilder();
			foreach (var value in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(url + privateKey)))
				stringBuilder.Append(value.ToString("x2"));
			return stringBuilder.ToString();
		}
	}
}