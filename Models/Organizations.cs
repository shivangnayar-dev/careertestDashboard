using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auxx.Models
{
    public class Organizations
    {
        [Key]
        public int OrganizationId { get; set; }

        [Required(ErrorMessage = "Organization name is required.")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "Superadmin email id is required.")]
        [Display(Name = "CG SuperAdmin EmailId")]
        public string CG_SuperAdminEmailId { get; set; }

        [Required(ErrorMessage = "Superadmin email id is required.")]
        [Display(Name = "Customer SuperAdmin EmailId")]
        public string Customer_SuperAdminEmailId { get; set; }

        [Required(ErrorMessage = "Admin email id is required.")]
        public string Customer_AdminEmailId1 { get; set; }
        [Required(ErrorMessage = "Admin email id is required.")]
        public string Customer_AdminEmailId2 { get; set; }
        [Required(ErrorMessage = "Admin email id is required.")]
        public string Customer_AdminEmailId3 { get; set; }

        public string? Customer_AdminEmailId4 { get; set; }

        //[Required(ErrorMessage = "Admin email id is required.")]
        public string? Customer_AdminEmailId5 { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        [NotMapped]
        public string? Exception_Msg { get; set; }
    }
}
