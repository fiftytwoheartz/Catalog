using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo {

    public interface IProductsRepo {

        Task<ResponseBuilder> AllByMetaCategory(int metaCategoryID);

        Task<ResponseBuilder> AllByMetaCategoryAndProductKind(
            int metaCategoryID,
            int productKindID);

        Task<ResponseBuilder> ById(
            int productId);

        Task<ResponseBuilder> Update(
            Product product);

    }

    public sealed class FakeProductsRepo : IProductsRepo {

        private static readonly IDictionary<int, Product> _DB = new Dictionary<int, Product> {
            {0, new Product {MetaCategoryID = 0, ID = 0, ProductKindID = 0, Title = "Product 0 title", FullDescription = "Full description of the product.", PriceInUSD = 150.50m, ShortDescription = "Short descroption of the product.", UniqueIdentifier = "P-0000"}}
        };

        public Task<ResponseBuilder> AllByMetaCategory(
            int metaCategoryID)
		{
			var res = WhereBelongsToMetaCategory(metaCategoryID);
			return Task.FromResult(res.Any() ? res.Success() : NoProductsMatchGivenCriteria());
		}

		private static ResponseBuilder NoProductsMatchGivenCriteria()
		{
			return "Not products match given criteria.".Failure();
		}

		private static IEnumerable<Product> WhereBelongsToMetaCategory(int metaCategoryID)
		{
			return _DB.Values.Where(product => product.MetaCategoryID == metaCategoryID);
		}

		public Task<ResponseBuilder> AllByMetaCategoryAndProductKind(int metaCategoryID, int productKindID)
		{
			var res = WhereBelongsToMetaCategory(metaCategoryID).Where(product => product.ProductKindID == productKindID);
			return Task.FromResult(res.Any() ? res.Success() : NoProductsMatchGivenCriteria());
		}

	    public Task<ResponseBuilder> ById(
            int productId)
		{
			return Task.FromResult(
				_DB.ContainsKey(productId)
					? _DB[productId].Success()
					: ProductDoesNotExistFailure(productId));
		}

		private static ResponseBuilder ProductDoesNotExistFailure(int productId)
		{
			return $"Product with ID={productId} does not exist.".Failure();
		}

		public Task<ResponseBuilder> Update(
            Product product) {
            return Task.FromResult(_DB.ContainsKey(product.ID) ? true.Success() : ProductDoesNotExistFailure(product.ID));
        }

    }

}