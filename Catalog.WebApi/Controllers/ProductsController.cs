using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using Catalog.Models;
using Catalog.Repo;

namespace Catalog.WebApi.Controllers {

    public class ProductsController : ApiController {

        private static readonly IProductsRepo _PRODUCTS_REPO = new FakeProductsRepo();

        private static readonly IMetaCategoriesRepo _META_CATEGORIES_REPO = new FakeMetaCategoriesRepo();

        //[HttpGet]
        //[Route("api/products/ofmetacategory/{metaCategoryID}")]
        //public async Task<IEnumerable<Product>> OfCategory(
        //    int metaCategoryID) {
        //    var metaCategory = await _META_CATEGORIES_REPO.ById(metaCategoryID);
        //    return await _PRODUCTS_REPO.AllByMetaCategory(metaCategory.ID);
        //}

        //[HttpGet]
        //[Route("api/products/ofmetacategory/{metaCategoryID}/ofproductkind/{productKindID}")]
        //public async Task<IEnumerable<Product>> OfCategoryAndProductType(
        //    int metaCategoryID,
        //    int productKindID) {
        //    var metaCategory = await _META_CATEGORIES_REPO.ById(metaCategoryID);
        //    return await _PRODUCTS_REPO.AllByMetacategoryAndProductKind(
        //        metaCategory.ID,
        //        productKindID);
        //}

        //[HttpGet]
        //[Route("api/products/{productID}")]
        //public async Task<JsonResult<Response>> ByID(
        //    int productID) {
        //    return Json(
        //        (await TryCatchHelper.TryCatchAsync(
        //            async () => (await _PRODUCTS_REPO.ById(productID)).Success())).Build());
        //}

        [HttpGet]
        [Route("api/admin/products/update/{productID}")]
        public async Task<JsonResult<Response>> UpdateByID(
            [FromUri] int productID,
            [FromBody] Product product) {
            product.ID = productID;
            var success = await _PRODUCTS_REPO.Update(product);
            if (success) {
                return Json(
                    product
                        .Success()
                        .Build());
            }

            return Json(
                "Failed to update given product.".Failure()
                    .Build());
        }

    }

}