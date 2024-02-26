using System.ComponentModel.DataAnnotations;

namespace Team09.Models
{
    public class Prospect
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? first_Name { get; set; }
        [Required]
        public string? last_Name { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? gender { get; set; }
        [Required]
        public float GPA { get; set; }
    }
}
