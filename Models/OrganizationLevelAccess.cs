using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auxx.Models
{
    public class OrganizationLevelAccess
    {
        [Key]
        public int OrgLevelAccesslId { get; set; }

        [NotMapped]
        public Organizations Organizations { get; set; }
        public int OrganizationId { get; set; }

        [NotMapped]
        public string OrganizationName { get; set; }

        public string AccessEmail_Core { get; set; }
        public string Levels_Core { get; set; }
        public string Enabled_Core { get; set; }
        public DateTime Startdate_Core { get; set; }
        public DateTime Enddate_Core { get; set; }
        public string Username_Core { get; set; }

        public string AccessEmail1 { get; set; }
        public string Levels_Email1 { get; set; }
        public string Enabled_Email1 { get; set; }
        public DateTime Startdate_Email1 { get; set; }
        public DateTime Enddate_Email1 { get; set; }
        public string Username_Email1 { get; set; }

        public string? AccessEmail2 { get; set; }
        public string? Levels_Email2 { get; set; }
        public string? Enabled_Email2 { get; set; }
        public DateTime? Startdate_Email2 { get; set; }
        public DateTime? Enddate_Email2 { get; set; }
        public string   Username_Email2 { get; set; }

        public string? AccessEmail3 { get; set; }
        public string? Levels_Email3 { get; set; }
        public string? Enabled_Email3 { get; set; }
        public DateTime? Startdate_Email3 { get; set; }
        public DateTime? Enddate_Email3 { get; set; }
        public string? Username_Email3 { get; set; }

        public string? AccessEmail4 { get; set; }
        public string? Levels_Email4 { get; set; }
        public string? Enabled_Email4 { get; set; }
        public DateTime? Startdate_Email4 { get; set; }
        public DateTime? Enddate_Email4 { get; set; }
        public string? Username_Email4 { get; set; }

        public string? AccessEmail5 { get; set; }
        public string? Levels_Email5 { get; set; }
        public string? Enabled_Email5 { get; set; }
        public DateTime? Startdate_Email5 { get; set; }
        public DateTime? Enddate_Email5 { get; set; }
        public string? Username_Email5 { get; set; }

        public string AccessEmail6 { get; set; }
        public string Levels_Email6 { get; set; }
        public string Enabled_Email6 { get; set; }
        public DateTime Startdate_Email6 { get; set; }
        public DateTime Enddate_Email6 { get; set; }
        public string Username_Email6 { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

    }

    public class OrganizationAccessViewModel
    {
        public OrganizationLevelAccess OrganizationLevelAccess { get; set; }
        public string OrganizationName { get; set; }

        public string Levelcore { get; set; }
        public string Levelname1 { get; set; }
        public string Levelname2 { get; set; }
        public string Levelname3 { get; set; }
        public string Levelname4 { get; set ; }    
        public string Levelname5 { get;set; }
        public string Levelname6 { get;set; }


    }

}