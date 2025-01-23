using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxx.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }
        public string Adhar { get; set; }

        public int Roleid { get; set; }

        public int Organizationid { get; set; }

        public int Level { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }

}