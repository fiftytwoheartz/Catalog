using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Models
{

    public class Product
    {
        public int ID { get; set; }
        public string UniqueIdentifier { get; set; }
        public int MetaCategoryID { get; set; }
        public int ProductKindID { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Title { get; set; }
        public decimal PriceInUSD { get; set; } 
    }
}
