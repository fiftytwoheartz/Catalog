using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using Catalog.Models;
using Catalog.Repo;

namespace Catalog.WebApi.Controllers
{
    public class CategoriesController : ApiController {

        private static IMetaCategoriesRepo _META_CATEGORIES_REPO = new FakeMetaCategoriesRepo();

        public async Task<JsonResult<Response>> Get() {
            return Json(
				(await _META_CATEGORIES_REPO.All()).Build());
        }

        public async Task<JsonResult<Response>> Get(int id)
		{
			return Json(
				(await _META_CATEGORIES_REPO.ById(id))
				.Hint(ProductsOfMetacategoryHint(id))
				.Build());
		}

		private static HintIntermediate ProductsOfMetacategoryHint(int id)
		{
			return new HintIntermediate(true, false, "Products of this metacategory:", $"api/categories/{id}/products");
		}

		public async Task<JsonResult<Response>> Post([FromBody]MetaCategory metaCategory) {
			return Json(
				(await _META_CATEGORIES_REPO.Register(metaCategory))
				.Hint(ProductsOfMetacategoryHint(metaCategory.ID))
				.Build());
        }

        public async Task<JsonResult<Response>> Delete(int id) {
            return Json((await _META_CATEGORIES_REPO.Unregiester(id)).Build());
        }
    }
}
