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

        private static readonly IDictionary<int, MetaCategory> _DB = new Dictionary<int, MetaCategory> {
            { 0, new MetaCategory { ID = 0, Description = "Description for metacategory 1.", Title = "MetaCategory 1" } }
        };

        public Task<ICollection<MetaCategory>> All() {
            return Task.FromResult(_DB.Values);
        }

        public Task<bool> Register(
            MetaCategory metaCategory) {
            _DB.Add(_DB.Count, metaCategory);
            return Task.FromResult(true);
        }

        public Task<bool> Unregiester(
            int ID) {
            if (_DB.ContainsKey(ID)) {
                _DB.Remove(ID);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<MetaCategory> ById(
            int ID) {
            return Task.FromResult(_DB[ID]);
        }

    }

}