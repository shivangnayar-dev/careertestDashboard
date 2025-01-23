using Humanizer;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace Auxx.Models
{
    public class OrganizationReports
    {
        [Key]
        public int OrganizationReportId { get; set; }

        //(from the List of reports- this table has 3 columns, GUID of report, Name of Report, Minimum cost)
        //from reportdetail table
        public int OrganizationId { get; set; }
        [NotMapped]
        public string OrganizationName { get; set; }
        public string ReportId { get; set; }

        //from reportdetail table
        [NotMapped]
        public string Reportname { get; set; }

        ////from reportdetail table (if nothing in List of report, set to INR 100)
        public int Minimumcostofreport { get; set; }

        //(as agreed in contract - default is INR 0)
        public int MarkuponMinimumcost { get; set; } = 0;

        //mincost of report + markupon min cost
        public int TotalCost { get; set; }

        [Required(ErrorMessage = "Contract start date is required.")]
        public DateTime Contract_Startdate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Contract end date is required.")]

        //End date(end of the contract, else default = 12 months)
        public DateTime Contract_Enddate { get; set; } = DateTime.Now.AddDays(30);

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
