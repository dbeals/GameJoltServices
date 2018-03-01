namespace GameJoltServices
{
	public abstract class ServiceBase
	{
		public ServicesManager Manager { get; private set; }

		protected ServiceBase(ServicesManager manager)
		{
			Manager = manager;
		}
	}
}