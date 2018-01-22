using System.Collections.Generic;

namespace Catalog.Repo
{
	public abstract class ResponseBuilder
	{
		public abstract Response Build();

		protected ICollection<HintIntermediate> _hints;

		protected ResponseBuilder()
		{
			_hints = new List<HintIntermediate>();
		}

		public ResponseBuilder Hint(HintIntermediate hintIntermediate)
		{
			_hints.Add(hintIntermediate);
			return this;
		}
	}

	public abstract class ResponseBuilder<T> : ResponseBuilder
	{
		protected readonly T _data;

		protected ResponseBuilder(T data)
		{
			_data = data;
		}
	}
}