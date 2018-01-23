using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Repo {

    public abstract class Response {

        public readonly bool Success;

	    public IEnumerable<Hint> Hints;

        protected Response(
            bool success,
			IEnumerable<Hint> hints) {
            Success = success;
			Hints = hints ?? Enumerable.Empty<Hint>();
        }

    }

	public sealed class FailedResponse : Response
	{
		public readonly string ErrorMessage;

		public FailedResponse(string errorMessage, IEnumerable<Hint> hints)
			: base(false, hints)
		{
			ErrorMessage = errorMessage;
		}
	}

    public sealed class SuccessfulResponse<T> : Response {

        public readonly T Data;

        public SuccessfulResponse(
			IEnumerable<Hint> hints, 
			T data) : base(true, hints) {
            Data = data;
        }

    }

}