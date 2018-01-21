namespace Catalog.Models {

    public class FailureResponseBuilder<T> : ResponseBuilder<T> {

        public FailureResponseBuilder(
            T data) : base(data) { }

        protected override Response<T> BuildGeneric() {
            return new Response<T>(false, _data);
        }

    }

}