using System;
using System.Threading;
using GameJolt;
using GameJoltServices.Requests;

namespace GameJoltServices.Saves
{
	public sealed class UploadSaveRequest : ServiceRequestBase<GameSaveService, bool>
	{
		public GameSave GameSave { get; set; }

		public UploadSaveRequest(GameSaveService service, GameSave gameSave)
			: base(service)
		{
			GameSave = gameSave;
		}

		public override bool Process(IAsyncResult result)
		{
			var token1 = Service.UpdateSaves(GameSave.Username, GameSave.UserToken, GameSave); // update the dictionary
			var token2 = DataStore.BeginSetByKey(Service.Manager.GameId, Service.Manager.PrivateKey, GameSave.Username, GameSave.UserToken, GameSave.Location, GameSave.SaveData); // upload the data
			while (!token1.IsCompleted && !token2.IsCompleted) Thread.Sleep(0);

			// We store the file locally AFTER the update so that the local copy has a newer timestamp and will be used over the cloud.
			Service.CacheSave(GameSave);
			return RequestHelper.WasSuccessful(DataStore.EndSetByKey(token2));
		}
	}
}