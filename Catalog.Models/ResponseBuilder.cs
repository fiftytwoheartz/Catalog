using System.Collections.Generic;

namespace Catalog.Models {

    public abstract class ResponseBuilder {

        public abstract Response Build();

    }

    public abstract class ResponseBuilder<T> : ResponseBuilder {

        protected readonly T _data;

        private readonly IList<Href> Hrefs;

        protected ResponseBuilder(T data) {
            Hrefs = new List<Href>();
            _data = data;
        }

        public override Response Build() {
            return BuildGeneric();
        }

        protected abstract Response<T> BuildGeneric();

    }

    public abstract class Href {

    }

}