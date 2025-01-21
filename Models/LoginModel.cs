using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxx.Models
{
    public class LoginModel
    {
        [NotMapped]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }
        public string Adhar { get; set; }

        public int Roleid { get; set; }

        public int Organizationid { get; set; }


    }

}
