using System.ComponentModel.DataAnnotations;

namespace Team09.Models
{
    public class Prospect
    {
        public int Id { get; set; }
        public string? first_Name { get; set; }
        public string? last_Name { get; set; }
        public string? email { get; set; }
        public string? gender { get; set; }
        public float GPA { get; set; }
    }
}
