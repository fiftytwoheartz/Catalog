using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using Catalog.Models;
using Catalog.Repo;

namespace Catalog.WebApi.Controllers {

    public class ProductsController : ApiController {

        private static readonly IProductsRepo _PRODUCTS_REPO = new FakeProductsRepo();

		[HttpGet]
		[Route("api/products/ofmetacategory/{metaCategoryID}")]
		public async Task<JsonResult<Response>> OfCategory(
			int metaCategoryID)
		{
			return Json(
				(await _PRODUCTS_REPO.AllByMetaCategory(metaCategoryID))
				.HintsBasedOnData<Product>(product => new Hint(HttpVerb.GET, "See product details.", $"api/products/{product.ID}"))
				.Build());
		}

		[HttpGet]
		[Route("api/products/ofmetacategory/{metaCategoryID}/ofproductkind/{productKindID}")]
		public async Task<JsonResult<Response>> OfCategoryAndProductType(
			int metaCategoryID,
			int productKindID)
		{
			return Json((await _PRODUCTS_REPO.AllByMetaCategoryAndProductKind(metaCategoryID, productKindID)).Build());
		}

		[HttpGet]
		[Route("api/products/{productID}")]
		public async Task<JsonResult<Response>> ByID(
			int productID)
		{
			return Json((await _PRODUCTS_REPO.ById(productID)).Success().Build());
		}

		[HttpGet]
        [Route("api/admin/products/update/{productID}")]
        public async Task<JsonResult<Response>> UpdateByID(
            [FromUri] int productID,
            [FromBody] Product product) {
            product.ID = productID;
			return Json((await _PRODUCTS_REPO.Update(product)).Build());
		}

    }

}