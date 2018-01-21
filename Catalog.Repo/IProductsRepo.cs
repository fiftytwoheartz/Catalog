using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo {

    public interface IProductsRepo {

        Task<IEnumerable<Product>> AllByMetaCategory(
            int metaCategoryID);

        Task<IEnumerable<Product>> AllByMetacategoryAndProductKind(
            int metaCategoryID,
            int productKindID);

        Task<Product> ById(
            int productId);

        Task<bool> Update(
            Product product);

    }

    public sealed class FakeProductsRepo : IProductsRepo {

        private static readonly IDictionary<int, Product> _DB = new Dictionary<int, Product> {
            {0, new Product {MetaCategoryID = 0, ID = 0, ProductKindID = 0, Title = "Product 0 title", FullDescription = "Full description of the product.", PriceInUSD = 150.50m, ShortDescription = "Short descroption of the product.", UniqueIdentifier = "P-0000"}}
        };

        public Task<IEnumerable<Product>> AllByMetaCategory(
            int metaCategoryID) {
            return Task.FromResult(_DB.Values.Where(product => product.MetaCategoryID == metaCategoryID));
        }

        public async Task<IEnumerable<Product>> AllByMetacategoryAndProductKind(
            int metaCategoryID,
            int productKindID) {
            return (await AllByMetaCategory(metaCategoryID)).Where(product => product.ProductKindID == productKindID);
        }

        public Task<Product> ById(
            int productId) {
            return Task.FromResult(_DB[productId]);
        }

        public Task<bool> Update(
            Product product) {
            if (_DB.ContainsKey(product.ID)) {
                _DB[product.ID] = product;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

    }

}