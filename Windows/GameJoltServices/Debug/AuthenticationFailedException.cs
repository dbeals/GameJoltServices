using System;

namespace GameJoltServices.Debug
{
	public sealed class AuthenticationFailedException : Exception
	{
		public string Username { get; private set; }

		public AuthenticationFailedException(string username)
			: this(username, null) { }

		public AuthenticationFailedException(string username, Exception innerException)
			: base($"Failed to authenticate user {username}.", innerException)
		{
			Username = username;
		}
	}
}