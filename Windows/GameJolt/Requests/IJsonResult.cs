using Newtonsoft.Json;

namespace GameJolt.Requests
{
	/// <summary>
	/// </summary>
	internal interface IJsonResult<TResult>
	{
		IJsonResponse<TResult> Response { get; }
	}

	internal interface IJsonResponse<TResult>
	{
		[JsonProperty("success")]
		string WasSuccessful { get; }
		string Message { get; }
		TResult ResultSet { get; }
	}
}