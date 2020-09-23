using System.Collections.Generic;
using System.Linq;

namespace CleanersAPI.Templates
{
    public class Invoice
    {
        public string Names { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; }

        public decimal TotalCost => InvoiceLines?.Sum(i => i.TotalCost) ?? 0;
    }
}