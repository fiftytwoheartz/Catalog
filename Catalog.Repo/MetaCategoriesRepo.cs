using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo {

    public interface MetaCategoriesRepo {

        Task<IList<MetaCategory>> All();

        Task<bool> Register(
            MetaCategory metaCategory);

        Task<bool> Unregiester(
            MetaCategory metaCategory);

    }

}