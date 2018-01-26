using System;
using System.Net.Http;
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
				.Hint(ProductsOfMetaCategoryHint(id))
				.Build());
		}

		private static HintIntermediate ProductsOfMetaCategoryHint(int id)
		{
			return new HintIntermediate(true, false, HttpVerb.GET, "See all the products associated with this meta-category.", $"api/categories/{id}/products");
		}

		public async Task<JsonResult<Response>> Post([FromBody]MetaCategory metaCategory) {
			return Json(
				(await _META_CATEGORIES_REPO.Register(metaCategory))
				.Hint(ProductsOfMetaCategoryHint(metaCategory.ID))
				.Build());
        }

        public async Task<JsonResult<Response>> Delete(int id) {
            return Json((await _META_CATEGORIES_REPO.Unregiester(id)).Build());
        }
    }
}
