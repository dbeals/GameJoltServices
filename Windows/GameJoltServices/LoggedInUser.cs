using System;
using System.Timers;
using GameJolt;

namespace GameJoltServices
{
	public sealed class LoggedInUser : User
	{
		#region Variables
		private readonly ServicesManager _manager;
		private readonly Timer _pingTimer;
		private float _autoPingDelay = 20000.0f;
		#endregion

		#region Properties
		public static LoggedInUser Guest => new LoggedInUser(null, new GameJolt.User
		{
			Username = "Guest"
		}, string.Empty)
		{
			IsGuest = true
		};

		public string UserToken { get; private set; }
		public UserIndex UserIndex { get; private set; }
		public Session Session { get; private set; }

		public SessionStatus SessionStatus
		{
			get => Session.Status;
			set => Session.Status = value;
		}

		public float AutoPingDelay
		{
			get => _autoPingDelay;
			set
			{
				if (IsGuest)
					return; // Cannot ping a guest.

				_autoPingDelay = value;
				_pingTimer.Interval = value;
				if (_autoPingDelay == 0.0f)
					_pingTimer.Stop();
				else
					_pingTimer.Start();
			}
		}

		public bool IsGuest { get; private set; }
		#endregion

		#region Constructors
		internal LoggedInUser(ServicesManager manager, GameJolt.User rawUser, string userToken, UserIndex userIndex = UserIndex.Any)
			: base(rawUser)
		{
			_manager = manager;
			UserToken = userToken;
			UserIndex = userIndex;

			if (manager == null)
				return;

			Session = new Session(rawUser.Username, userToken);

			_pingTimer = new Timer {Interval = _autoPingDelay};
			_pingTimer.Elapsed += (sender, args) =>
			{
				if (Session.IsValid)
					Session.BeginPing(manager.GameId, manager.PrivateKey);
			};
		}
		#endregion

		#region Methods
		public IAsyncResult BeginGetScores(string tableId, AsyncCallback callback = null, object asyncState = null)
		{
			return IsGuest ? null : Score.BeginGetScores(_manager.GameId, _manager.PrivateKey, Username, UserToken, 0, tableId, callback, asyncState);
		}

		public Score[] EndGetScores(IAsyncResult result)
		{
			if (IsGuest)
				return null;

			return Score.EndGetScores(result) ?? new Score[0];
		}

		public Score[] GetScores(string tableId)
		{
			if (IsGuest)
				return null;

			return Score.GetScores(_manager.GameId, _manager.PrivateKey, Username, UserToken, 0, tableId) ?? new Score[0];
		}

		public IAsyncResult GetScores(string tableId, Action<Score[]> callback)
		{
			return IsGuest ? null : Score.GetScores(_manager.GameId, _manager.PrivateKey, Username, UserToken, 0, tableId, callback);
		}

		public IAsyncResult BeginGetTrophy(string trophyId, AsyncCallback callback = null, object asyncState = null)
		{
			return IsGuest ? null : Trophy.BeginGetById(_manager.GameId, _manager.PrivateKey, Username, UserToken, trophyId, callback, asyncState);
		}

		public Trophy EndGetTrophy(IAsyncResult result)
		{
			return IsGuest ? null : Trophy.EndGetById(result);
		}

		public Trophy GetTrophy(string trophyId)
		{
			return IsGuest ? null : Trophy.GetById(_manager.GameId, _manager.PrivateKey, Username, UserToken, trophyId);
		}

		public IAsyncResult BeginGetTrophies(TrophyFilter filter, AsyncCallback callback = null, object asyncState = null)
		{
			return IsGuest ? null : Trophy.BeginGetAll(_manager.GameId, _manager.PrivateKey, Username, UserToken, filter, callback, asyncState);
		}

		public Trophy[] EndGetTrophies(IAsyncResult result)
		{
			return IsGuest ? null : Trophy.EndGetAll(result);
		}

		public Trophy[] GetTrophies(TrophyFilter filter)
		{
			return IsGuest ? null : Trophy.GetAll(_manager.GameId, _manager.PrivateKey, Username, UserToken, filter);
		}

		public IAsyncResult GetTrophies(TrophyFilter filter, Action<Trophy[]> callback)
		{
			return IsGuest ? null : Trophy.GetAll(_manager.GameId, _manager.PrivateKey, Username, UserToken, filter, callback);
		}

		public IAsyncResult BeginUnlockTrophy(string trophyId, AsyncCallback callback = null, object asyncState = null)
		{
			if (IsGuest)
				return null;

			return Trophy.BeginGetById(_manager.GameId, _manager.PrivateKey, Username, UserToken, trophyId, result =>
			{
				var info = Trophy.EndGetById(result);
				if (info.Achieved == "false")
					Trophy.BeginAddTrophy(_manager.GameId, _manager.PrivateKey, Username, UserToken, trophyId, callback, asyncState);
			});
		}

		public AddTrophyResult EndUnlockTrophy(IAsyncResult result)
		{
			if (IsGuest)
				return AddTrophyResult.IsGuest;

			var addResult = Trophy.EndAddTrophy(result);
			return RequestHelper.WasSuccessful(addResult) ? AddTrophyResult.Success : AddTrophyResult.AlreadyAchieved;
		}

		public AddTrophyResult UnlockTrophy(string trophyId)
		{
			if (IsGuest)
				return AddTrophyResult.IsGuest;

			var result = Trophy.GetById(_manager.GameId, _manager.PrivateKey, Username, UserToken, trophyId);
			if (result.Achieved != "false")
				return AddTrophyResult.AlreadyAchieved;

			var addResult = Trophy.AddTrophy(_manager.GameId, _manager.PrivateKey, Username, UserToken, trophyId);
			return RequestHelper.WasSuccessful(addResult) ? AddTrophyResult.Success : AddTrophyResult.Failure;
		}
		#endregion
	}
}