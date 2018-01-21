namespace Catalog.Models {

    public static class Extensions {

        public static ResponseBuilder Failure<T>(
            this T data) {
            return new FailureResponseBuilder<T>(data);
        }

        public static ResponseBuilder Success<T>(
            this T data) {
            return new SuccessResponseBuilder<T>(data);
        }

    }

}