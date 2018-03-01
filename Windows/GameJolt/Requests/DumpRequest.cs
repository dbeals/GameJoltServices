namespace GameJolt.Requests
{
	internal sealed class DumpRequest : GenericRequestBase<string>
	{
		public bool WasSuccessful { get; set; }

		public DumpRequest(string gameId, string privateKey, string url, params RequestParameter[] parameters)
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

			var components = data.Split('\n');
			WasSuccessful = RequestHelper.WasSuccessful(components[0]);
			return components[1];
		}
	}
}