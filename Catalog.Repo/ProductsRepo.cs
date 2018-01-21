using System.Collections.Generic;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo {

    public interface ProductsRepo {

        Task<IList<Product>> AllByMetacategory(
            int metaCategoryID);

        Task<IList<Product>> AllByMetacategoryAndProductKind(
            int metaCategoryID,
            int productKindID);

    }

}