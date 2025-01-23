using static System.Net.Mime.MediaTypeNames;
using System.Composition;
using System.Xml.Linq;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auxx.Models
{
    public class Invoices
    {
        public int Id { get; set; }

        [NotMapped]
        public Organizations Organizations { get; set; }
        [NotMapped]
        public OrganizationReports OrganizationReports { get; set; }

        //Forienge key of oganization
        public int OrganizationId { get; set; }

        //Forienge key of Reports
        public int ReportId { get; set; }

        public string Codeofreport { get; set; }
        public string Countoftests { get; set; }
        public string Countofreports { get; set; }

        public string TotalCost { get; set; }

        //(Total cost *count of test) 
        public string SumofCost { get; set; }
        public string InvoiceGUID { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
