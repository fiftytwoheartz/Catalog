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
        public MetaCategory MetaCategory { get; set; }
        public ProductKind ProductKind { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Title { get; set; }
        public decimal PriceInUSD { get; set; } 
    }
}
