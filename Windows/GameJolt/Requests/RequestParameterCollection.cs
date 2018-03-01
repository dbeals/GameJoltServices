using System.Collections.ObjectModel;

namespace GameJolt.Requests
{
	internal sealed class RequestParameterCollection : KeyedCollection<string, RequestParameter>
	{
		public new object this[string parameterName]
		{
			get => base[parameterName]?.Value;
			set
			{
				RequestParameter parameter;
				if (!Contains(parameterName))
				{
					parameter = new RequestParameter(parameterName, value);
					Add(parameter);
				}
				else
				{
					parameter = base[parameterName];
					parameter.Value = value;
				}
			}
		}

		public void Add(string name, object value)
		{
			Add(new RequestParameter(name, value));
		}

		protected override string GetKeyForItem(RequestParameter item)
		{
			return item.Name;
		}
	}
}