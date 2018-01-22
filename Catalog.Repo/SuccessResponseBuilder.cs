using System.Linq;

namespace Catalog.Repo {

    public class SuccessResponseBuilder<T> : ResponseBuilder<T> {

        public SuccessResponseBuilder(
            T data) : base(data) { }

	    public override Response Build()
	    {
		    return new SuccessfulResponse<T>(_hints.Where(hint => hint.StaysForSuccess).Select(hint => hint.Build()), _data);
	    }
    }

}