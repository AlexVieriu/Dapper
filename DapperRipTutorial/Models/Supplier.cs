using System.Collections.Generic;

namespace DapperRipTutorial.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactName { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
    }
}
