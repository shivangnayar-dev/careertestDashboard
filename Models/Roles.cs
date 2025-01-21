using System.ComponentModel.DataAnnotations;

namespace Auxx.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role name is required.")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Role Description is required.")]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
