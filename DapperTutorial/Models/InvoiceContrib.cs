using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;


namespace DapperTutorial.Models
{
    [Table("Invoice")]
	public class InvoiceContrib
	{
		public int InvoiceID { get; set; }

		public string Code { get; set; }
		//public InvoiceKind Kind { get; set; }

		[Write(false)]
		[Computed]
		public string FakeProperty { get; set; }
	}
}
