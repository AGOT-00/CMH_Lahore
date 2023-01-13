using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CMH_Lahore.Models
{
    public class Complaint
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string cname { get; set; }
        public string cnic { get; set; }
        
        public DateTime DOC { get; set; } = DateTime.Now;

        [Required]
        [Phone]
        public string phone { get; set; }

        [Required]
        public string DocName
        { get; set; }
        
        [Required]
        public int Department
        { get; set; }
        
        [Required]
        public DateTime DOI { get; set; }

        [AllowNull]
        public string ComplaintType
        { get; set; }

        [Required]
        public string ComplaintDesc
        { get; set; }

        public int status { get; set; } = 0;

        [AllowNull]
        public DateTime DOR { get; set; }
        
        public string getstatus()
        {
            return status switch
            {
                0 => "Open",
                1 => "Forwaded",
                2 => "Allocated",
                3 => "Partially Resolved",
                4 => "Resolved",
                5 => "Considered Junk/Fake",
                _ => "Unknown",
            };
        }
    }

    public class explaination
    {
        [Key]
        [ForeignKey("Complaint")]
        public int id { get; set; }

        public string explain { get; set; }

    }

    public class comment {
        [ForeignKey("Complaint")]
        public int id { get; set; }
        public string commentername { get; set; }
        public string comments { get; set; }

        public comment()
        {
            
        }
        public comment(int _id, string _commentername, string comments)
        {
            this.id = _id;
            this.commentername = _commentername;
            this.comments = comments;
        }
    }
}
