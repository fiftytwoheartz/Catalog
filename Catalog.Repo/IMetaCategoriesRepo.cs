using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo {

    public interface IMetaCategoriesRepo {

        Task<ICollection<MetaCategory>> All();

        Task<bool> Register(
            MetaCategory metaCategory);

        Task<bool> Unregiester(
            int ID);

        Task<MetaCategory> ById(
            int ID);

    }

    public sealed class FakeMetaCategoriesRepo : IMetaCategoriesRepo {

        private static readonly IDictionary<int, MetaCategory> _db = new Dictionary<int, MetaCategory> {
            { 0, new MetaCategory { ID = 0, Description = "Description for metacategory 1.", Title = "MetaCategory 1" } }
        };

        public Task<ICollection<MetaCategory>> All() {
            return Task.FromResult(_db.Values);
        }

        public Task<bool> Register(
            MetaCategory metaCategory) {
            _db.Add(_db.Count, metaCategory);
            return Task.FromResult(true);
        }

        public Task<bool> Unregiester(
            int ID) {
            if (_db.ContainsKey(ID)) {
                _db.Remove(ID);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<MetaCategory> ById(
            int ID) {
            return Task.FromResult(_db[ID]);
        }

    }

}