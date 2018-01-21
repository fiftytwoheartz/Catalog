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
    public class CategoriesController : ApiController {

        private static IMetaCategoriesRepo _META_CATEGORIES_REPO = new FakeMetaCategoriesRepo();

        public async Task<IEnumerable<MetaCategory>> Get() {
            return await _META_CATEGORIES_REPO.All();
        }

        public async Task<MetaCategory> Get(int id) {
            return await _META_CATEGORIES_REPO.ById(id);
        }

        public async Task<bool> Post([FromBody]MetaCategory metaCategory) {
            return await _META_CATEGORIES_REPO.Register(metaCategory);
        }

        public async Task<bool> Delete(int id) {
            return await _META_CATEGORIES_REPO.Unregiester(id);
        }
    }
}
