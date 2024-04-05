using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdigyScout.Models
{
    public class ComplexDetails
    {
        [Key]
        [ForeignKey("Prospect")]
        public int ProspectId { get; set; }
        public bool IsWatched { get; set; }
        public bool IsPipeline { get; set; }
        public virtual Prospect Prospect { get; set; }
    }
}
