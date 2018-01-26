using System;
using System.Linq;

namespace Catalog.Repo
{
	public class FailureResponseBuilder : ResponseBuilder
	{
		public readonly string ErrorMessage;

		public FailureResponseBuilder(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}

		public override Response Build()
		{
			return new FailedResponse(ErrorMessage, _hints.Where(hint => hint.StaysForFailure).Select(hint => hint.Build()));
		}

		public override ResponseBuilder HintsBasedOnData<TData>(Func<TData, Hint> pattern)
		{
			return this;
		}
	}
}