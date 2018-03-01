namespace GameJolt.Requests
{
	internal sealed class StringRequest : GenericRequestBase<string>
	{
		public StringRequest(string gameId, string privateKey, string url, params RequestParameter[] parameters)
			: base(gameId, privateKey)
		{
			UrlBase = url;
			foreach (var parameter in parameters)
				Parameters[parameter.Name] = parameter.Value;
		}

		protected override string ProcessResponseData(string data)
		{
			if (Logging.IsEnabled)
				Logging.LogInformation(data);
			return data;
		}
	}
}