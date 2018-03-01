using System;
using System.IO;
using System.Net;

namespace GameJolt.Requests
{
	internal abstract class GenericRequestBase<TResult> : WebRequestBase<TResult>
	{
		protected GenericRequestBase(string gameId, string privateKey)
			: base(gameId, privateKey) { }

		public override TResult Process(IAsyncResult result)
		{
			base.Process(result);
			var fullUrl = GetFullUrl();
			var request = (HttpWebRequest) WebRequest.Create(fullUrl);
			request.Method = "GET";
			try
			{
				using (var response = request.GetResponse())
				{
					using (var streamReader = new StreamReader(response.GetResponseStream()))
					{
						var data = streamReader.ReadToEnd();
						return ProcessResponseData(data);
					}
				}
			}
			catch (WebException)
			{
				return default(TResult);
			}
		}

		protected abstract TResult ProcessResponseData(string data);
	}
}