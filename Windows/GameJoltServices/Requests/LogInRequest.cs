using System;
using GameJolt;
using GameJolt.Async;

namespace GameJoltServices.Requests
{
	public sealed class LogInRequest : AsyncRequestBase<LoggedInUser>
	{
		public ServicesManager Manager { get; private set; }

		public override string GameId
		{
			get => Manager.GameId;
			set { }
		}

		public string Username { get; private set; }
		public string UserToken { get; private set; }
		public UserIndex UserIndex { get; private set; }

		protected override string PrivateKey
		{
			get => Manager.PrivateKey;
			set { }
		}

		public LogInRequest(ServicesManager manager, string username, string userToken, UserIndex userIndex)
			: base(null, null)
		{
			Manager = manager;
			Username = username;
			UserToken = userToken;
		}

		public override LoggedInUser Process(IAsyncResult result)
		{
			if (Manager.IsUsingDevData)
			{
				var tokens = UserToken.Split(' ');
				var username = Username;
				var type = "User";
				var status = "Active";
				foreach (var token in tokens)
				{
					if (token.StartsWith("type"))
						type = token.Substring(token.IndexOf(':') + 1);
					else if (token.StartsWith("status"))
						status = token.Substring(token.IndexOf(':') + 1);
					else if (token.StartsWith("username"))
						username = token.Substring(token.IndexOf(':') + 1);
				}

				return new LoggedInUser(Manager, new GameJolt.User
				{
					Username = username,
					Type = type,
					Status = status,
					SignedUp = "1 Month ago",
					LastLoggedIn = "Online Now",
					DeveloperName = type == "Developer" ? "Fake Games Inc." : string.Empty,
					DeveloperWebsite = type == "Developer" ? "fakegamesinc.com" : string.Empty,
					DeveloperDescription = type == "Developer" ? "We're not a real game development company." : string.Empty
				}, UserToken);
			}

			var authResult = GameJolt.User.AuthenticateUser(GameId, PrivateKey, Username, UserToken);
			if (!RequestHelper.WasSuccessful(authResult))
				return null;

			var rawUser = GameJolt.User.GetByUsername(GameId, PrivateKey, Username);
			return new LoggedInUser(Manager, rawUser, UserToken, UserIndex);
		}
	}
}