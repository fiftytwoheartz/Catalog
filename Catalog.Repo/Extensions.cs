namespace Catalog.Repo {

    public static class Extensions {

        public static ResponseBuilder Failure(
            this string errorMessage) {
            return new FailureResponseBuilder(string.IsNullOrWhiteSpace(errorMessage) ? "No message provided..." : errorMessage);
        }

        public static ResponseBuilder Success<T>(
            this T data)
        {
	        return data == null ? "Null.".Failure() : new SuccessResponseBuilder<T>(data);
        }

    }

}