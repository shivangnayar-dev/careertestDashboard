using System.ComponentModel.DataAnnotations;

namespace Auxx.Models
{
    public class Levels
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        public string LevelName { get; set; }
    }
}
