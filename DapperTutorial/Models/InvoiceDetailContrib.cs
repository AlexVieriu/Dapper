using Dapper.Contrib.Extensions;

namespace DapperTutorial.Models
{
    [Table("InvoiceDetail")]
    public class InvoiceDetailContrib
    {
        [ExplicitKey]
        public int InvoiceID { get; set; }

        public string Detail { get; set; }
    }
}
