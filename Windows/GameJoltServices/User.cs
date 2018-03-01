using System;
using System.IO;
using GameJolt;

namespace GameJoltServices
{
	public class User
	{
		public string UserId { get; private set; }
		public UserType UserType { get; private set; }
		public string Username { get; private set; }
		public string AvatarUrl { get; private set; }
		public string SignedUp { get; private set; }
		public string LastLoggedIn { get; private set; }
		public UserAccountStatus AccountStatus { get; private set; }
		public string DeveloperName { get; private set; }
		public string DeveloperWebsite { get; private set; }
		public string DeveloperDescription { get; private set; }

		public User(GameJolt.User rawUser)
		{
			UserId = rawUser.Id;
			UserType = string.IsNullOrWhiteSpace(rawUser.Type) ? UserType.Unknown : (UserType) Enum.Parse(typeof(UserType), rawUser.Type);
			Username = rawUser.Username;
			AvatarUrl = rawUser.AvatarUrl;
			SignedUp = rawUser.SignedUp;
			LastLoggedIn = rawUser.LastLoggedIn;
			AccountStatus = string.IsNullOrWhiteSpace(rawUser.Status) ? UserAccountStatus.Unknown : (UserAccountStatus) Enum.Parse(typeof(UserAccountStatus), rawUser.Status);
			DeveloperName = rawUser.DeveloperName;
			DeveloperWebsite = rawUser.DeveloperWebsite;
			DeveloperDescription = rawUser.DeveloperDescription;
		}

		public IAsyncResult BeginGetAvatar(AsyncCallback callback = null, object asyncState = null)
		{
			return RequestHelper.BeginGetImage(AvatarUrl, callback, asyncState);
		}

		public Stream EndGetAvatar(IAsyncResult result)
		{
			return RequestHelper.EndGetImage(result);
		}

		public Stream GetAvatar()
		{
			return RequestHelper.GetImage(AvatarUrl);
		}

		public IAsyncResult GetAvatar(Action<Stream> callback)
		{
			return RequestHelper.GetImage(AvatarUrl, callback);
		}
	}
}