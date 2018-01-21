namespace Catalog.Models {

    public class SuccessResponseBuilder<T> : ResponseBuilder<T> {

        public SuccessResponseBuilder(
            T data) : base(data) { }

        protected override Response<T> BuildGeneric() {
            return new Response<T>(true, _data);
        }

    }

}