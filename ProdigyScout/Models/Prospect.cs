namespace ProdigyScout.Models
{
    public class Prospect
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string Gender { get; set; }
        public float GPA { get; set; }
        public DateTime GraduationDate { get; set; }

        public string GraduationDateFormatted
        {
            get { return string.Concat(GraduationDate.ToString("MMM"), " ", GraduationDate.ToString("yyyy")); }
        }
    }
}
