using System.ComponentModel.DataAnnotations;

namespace Auxx.Models
{
    public class CGStagingData
    {
        [Key]
        public int CGstagid { get; set; }
        public int CandidateId { get; set; }
        public string TestCode { get; set; }
        public DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public string Name { get; set; }
        public string organization { get; set; }
        public string ReportName { get; set; }
        public string reporturl { get; set; }
    }
}
