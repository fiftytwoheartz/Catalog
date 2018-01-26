using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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

		public abstract ResponseBuilder HintsBasedOnData<TData>(Func<TData, Hint> pattern);
	}

	public abstract class ResponseBuilder<T> : ResponseBuilder
	{
		protected readonly T _data;

		protected ResponseBuilder(T data)
		{
			_data = data;
		}

		public override ResponseBuilder HintsBasedOnData<TData>(Func<TData, Hint> pattern)
		{
			return TryGenerateHintIntermediateFromPattern(_data, pattern) // if _data is Enumerable itself
				       ? TryGenerateHintIntermediateFromPattern(Enumerable.Repeat(_data, 1), pattern) // forcing non-Enumerable to became one
					         ? this
					         : this
				       : this;
		}

		private bool TryGenerateHintIntermediateFromPattern<TData>(object data, Func<TData, Hint> pattern)
		{
			var dataAsEnumerable = data as IEnumerable;
			if (dataAsEnumerable != null)
			{
				foreach (var hint in dataAsEnumerable.OfType<TData>().Select(pattern))
				{
					Hint(
						new HintIntermediate(
							true,
							false,
							(HttpVerb)Enum.Parse(typeof(HttpVerb), hint.HttpMethod),
							hint.Description,
							hint.RelativeURL));
				}

				return true;
			}

			return false;
		}
	}
}