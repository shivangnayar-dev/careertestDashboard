using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auxx.Models
{
    public class FunctionAccessControl
    {
        [Key]
        public int FunctionAccessId { get; set; }

        [NotMapped]
        public Roles Roles { get; set; }
        [NotMapped]
        public Functions Functions { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public string SuperAdmin_CG { get; set; }
        public string SuperAdmin_Customer { get; set; }
        public string Admin { get; set; }
        public string User { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
