using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auxx.Models;

public partial class Reportdetails
{
    [NotMapped]
    public int Id { get; set; }
    public string? Name { get; set; }
    [Key]
    public string? ReportId { get; set; }

    public int? Cost { get; set; }

    //public DateTime CreatedDate { get; set; }
    //public string CreatedBy { get; set; }

    //public DateTime? UpdatedDate { get; set; }
    //public string? UpdatedBy { get; set; }
}
