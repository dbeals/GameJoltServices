#region File Header
/***********************************************************************
 * Copyright © 2013 Beals Software
 * All Rights Reserved
************************************************************************
Author: Donald Beals
Date: February 4th, 2013
Description: TODO: Write a description of this file here.
****************************** Change Log ******************************
02.04.13 - Created initial file. (dbeals)
02.18.13 - Added GetStringRequestData() and GetJsonRequestData() to allow accessing request data without exposing request classes.
***********************************************************************/
#endregion

#region Using Statements
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GameJolt.Requests;
using GameJolt.Async;
#endregion

namespace GameJolt
{
	/// <summary>
	/// 
	/// </summary>
	public static class Helper
	{
		public static class UserUrls
		{
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/users/?format=json";
			public static readonly string AuthUrl = @"http://gamejolt.com/api/game/v1/users/auth/?format=dump";
		}

		public static class SessionUrls
		{
			public static readonly string OpenUrl = @"http://gamejolt.com/api/game/v1/sessions/open/?format=dump";
			public static readonly string PingUrl = @"http://gamejolt.com/api/game/v1/sessions/ping/?format=dump";
			public static readonly string CloseUrl = @"http://gamejolt.com/api/game/v1/sessions/close/?format=dump";
		}

		public static class TrophyUrls
		{
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/trophies/?format=json";
			public static readonly string AddAchieveUrl = @"http://gamejolt.com/api/game/v1/trophies/add-achieved/?format=dump";
		}

		public static class DataStoreUrls
		{
			public static readonly string GetKeysUrl = @"http://gamejolt.com/api/game/v1/data-store/get-keys?format=json";
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/data-store/?format=dump";
			public static readonly string SetUrl = @"http://gamejolt.com/api/game/v1/data-store/set/?format=dump";
			public static readonly string UpdateUrl = @"http://gamejolt.com/api/game/v1/data-store/update/?format=dump";
			public static readonly string RemoveUrl = @"http://gamejolt.com/api/game/v1/data-store/remove/?format=dump";
		}

		public static class ScoreUrls
		{
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/scores/?format=json";
			public static readonly string AddUrl = @"http://gamejolt.com/api/game/v1/scores/add/?format=dump";
			public static readonly string TablesUrl = @"http://gamejolt.com/api/game/v1/scores/tables/?format=json";
		}

		public static bool WasSuccessful(string result)
		{
			if(string.IsNullOrWhiteSpace(result))
				return false;

			return result.Trim().ToLower() == "success";
		}

		internal static string CreateSignature(string url, string privateKey)
		{
			var stringBuilder = new StringBuilder();
			foreach(var value in MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(url + privateKey)))
				stringBuilder.Append(value.ToString("x2"));
			return stringBuilder.ToString();
		}

		public static IAsyncResult BeginGetImage(string imageUrl, AsyncCallback callback = null, object asyncState = null)
		{
			var request = new ImageRequest(imageUrl);
			return request.Begin(callback, asyncState);
		}

		public static Stream EndGetImage(IAsyncResult result)
		{
			var joltResult = (AsyncResult<Stream>)result;
			var request = (ImageRequest)joltResult.CoreData;
			return request.End(result);
		}

		public static Stream GetImage(string imageUrl)
		{
			var request = new ImageRequest(imageUrl);
			return request.Process(null);
		}

		public static IAsyncResult GetImage(string imageUrl, Action<Stream> callback)
		{
			return BeginGetImage(imageUrl, (result) => callback(EndGetImage(result)));
		}

		public static string GetStringRequestData(IAsyncResult result)
		{
			var joltResult = (AsyncResultNoResult)result;
			var request = (StringRequest)joltResult.CoreData;
			return request.End(result);
		}

		public static TResult GetJsonRequestData<TResult>(IAsyncResult result)
		{
			var joltResult = (AsyncResultNoResult)result;
			var request = (JsonRequest<TResult, IJsonResult<TResult>>)joltResult.CoreData;
			return request.End(result);
		}
	}
}
