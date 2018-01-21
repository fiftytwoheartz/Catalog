using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Catalog.Models;
using Catalog.Repo;

namespace Catalog.WebApi.Controllers
{
    public class ProductsController : ApiController {

        private static readonly IProductsRepo _PRODUCTS_REPO = new FakeProductsRepo();

        private static readonly IMetaCategoriesRepo _META_CATEGORIES_REPO = new FakeMetaCategoriesRepo();

        [HttpGet]
        [Route("api/products/ofmetacategory/{metaCategoryID}")]
        public async Task<IEnumerable<Product>> OfCategory(int metaCategoryID) {
            var metaCategory = await _META_CATEGORIES_REPO.ById(metaCategoryID);
            return await _PRODUCTS_REPO.AllByMetaCategory(metaCategory.ID);
        }

        [HttpGet]
        [Route("api/products/ofmetacategory/{metaCategoryID}/ofproductkind/{productKindID}")]
        public async Task<IEnumerable<Product>> OfCategoryAndProductType(
            int metaCategoryID,
            int productKindID) {
            var metaCategory = await _META_CATEGORIES_REPO.ById(metaCategoryID);
            return await _PRODUCTS_REPO.AllByMetacategoryAndProductKind(metaCategory.ID, productKindID);
        }

        [HttpGet]
        [Route("api/products/{productID}")]
        public async Task<Product> ByID(
            int productID) {
            return await _PRODUCTS_REPO.ById(productID);
        }

    }
}
