namespace ProdigyScout.Models
{
    public class Prospect
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public float GPA { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Degree { get; set; }
        public string ResumePath { get; set; }

        public string GraduationDateFormatted
        {
            get { return string.Concat(GraduationDate.ToString("MMM"), " ", GraduationDate.ToString("yyyy")); }
        }

        public virtual ComplexDetails ComplexDetails { get; set; }
    }
}
