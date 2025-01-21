using System.ComponentModel.DataAnnotations;

namespace Auxx.Models
{
    public class Functions
    {
        [Key]
        public int FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string FunctionDescription { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
