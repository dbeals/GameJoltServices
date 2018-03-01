using System;
using Newtonsoft.Json;

namespace GameJolt.Requests
{
	internal class JsonRequest<TResult, TJsonResult> : GenericRequestBase<TResult>
		where TJsonResult : IJsonResult<TResult>
	{
		public JsonRequest(string gameId, string privateKey, string url, params RequestParameter[] parameters)
			: base(gameId, privateKey)
		{
			UrlBase = url;
			foreach (var parameter in parameters)
				Parameters[parameter.Name] = parameter.Value;
		}

		protected override TResult ProcessResponseData(string data)
		{
			if (Logging.IsEnabled)
				Logging.LogInformation(data);

			var jsonResult = JsonConvert.DeserializeObject<TJsonResult>(data);

			if (!bool.Parse(jsonResult.Response.WasSuccessful))
				throw new Exception(jsonResult.Response.Message);
			return jsonResult.Response.ResultSet;
		}
	}
}