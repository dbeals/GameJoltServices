using System;
using System.Linq;
using GameJolt.Async;

namespace GameJolt.Requests
{
	internal abstract class WebRequestBase<TResult> : AsyncRequestBase<TResult>
	{
		public override string GameId
		{
			get => GetParameterString("game_id");
			set => Parameters["game_id"] = value;
		}

		public string UrlBase { get; protected set; }

		public RequestParameterCollection Parameters { get; } = new RequestParameterCollection();

		protected WebRequestBase(string gameId, string privateKey)
			: base(gameId, privateKey) { }

		public override TResult Process(IAsyncResult result)
		{
			if (Logging.IsEnabled)
				Logging.LogInformation(GetFullUrl());
			return default(TResult);
		}

		protected string GetParameterString(string parameterName, string defaultValue = null)
		{
			if (Parameters.Contains(parameterName))
				return defaultValue;
			return (string) Parameters[parameterName];
		}

		protected virtual void UpdateParameters() { }

		protected string GetFullUrl()
		{
			UpdateParameters();
			var output = UrlBase;
			var parametersString = string.Join<RequestParameter>("&", Parameters.ToArray());

			if (!string.IsNullOrWhiteSpace(parametersString))
				output = AppendParameter(output, parametersString);

			var signature = RequestHelper.CreateSignature(output, PrivateKey);
			output = AppendParameter(output, "signature", signature);

			return Uri.EscapeUriString(output);
		}

		private static string AppendParameter(string url, string parameter)
		{
			return url + (url.Contains('?') ? "&" : "?") + parameter;
		}

		private static string AppendParameter(string url, string name, string value)
		{
			return AppendParameter(url, $"{name}={value}");
		}
	}
}