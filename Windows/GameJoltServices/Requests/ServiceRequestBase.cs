using GameJolt.Async;

namespace GameJoltServices.Requests
{
	public class ServiceRequestBase<TService, TResult> : AsyncRequestBase<TResult>
		where TService : ServiceBase
	{
		public TService Service { get; private set; }

		public ServiceRequestBase(TService service)
			: base(service.Manager.GameId, service.Manager.PrivateKey)
		{
			Service = service;
		}
	}
}