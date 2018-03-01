using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameJolt
{
	public sealed class UserCollection : KeyedCollection<string, User>
	{
		public UserCollection() { }

		public UserCollection(IEnumerable<User> users)
		{
			AddRange(users);
		}

		public void AddRange(IEnumerable<User> users)
		{
			foreach (var user in users)
				Add(user);
		}

		protected override string GetKeyForItem(User item)
		{
			return item.Id;
		}
	}
}