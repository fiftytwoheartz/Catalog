using System.Collections.Generic;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.Repo {

    public interface IProductsRepo {

        Task<IList<Product>> AllByMetacategory(
            int metaCategoryID);

        Task<IList<Product>> AllByMetacategoryAndProductKind(
            int metaCategoryID,
            int productKindID);

    }

}