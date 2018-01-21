using System.Runtime.InteropServices.ComTypes;

using Newtonsoft.Json;

namespace Catalog.Models {

    public abstract class Response {

        public readonly bool Success;

        protected Response(
            bool success) {
            Success = success;
        }

        public string AsJson() {
            return JsonConvert.SerializeObject(this);
        }

    }

    public sealed class Response<T> : Response {


        public readonly T Data;


        public Response(
            bool success,
            T data) : base(success) {
            Data = data;
        }

    }

}