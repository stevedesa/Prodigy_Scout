using ProdigyScout.Interfaces;
using ProdigyScout.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProdigyScout.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Gender = string.Empty;
            EmailID = string.Empty;
            GPA = 0;
            GraduationDate = DateTime.Today;
        }

        public StudentViewModel(Prospect student)
        {
            if (student != null)
            {
                Id = student.Id;
                FirstName = student.FirstName;
                LastName = student.LastName;
                EmailID = student.Email;
                Gender = student.Gender;
                GPA = student.GPA;
                GraduationDate = student.GraduationDate;
                IsWatched = student.ComplexDetails != null && student.ComplexDetails.IsWatched;
            }
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Email ID")]
        public string EmailID { get; set; }

        [Required]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        [Required]
        [DisplayName("GPA")]
        public float GPA { get; set; }

        [Required]
        [DisplayName("Graduation Date")]
        [DataType(DataType.Date)]
        public DateTime GraduationDate { get; set; }

        public string GraduationDateFormatted
        {
            get { return string.Concat(GraduationDate.ToString("MMM"), " ", GraduationDate.ToString("yyyy")); }
        }

        public bool IsWatched { get; set; }

        public IEnumerable<Prospect> Students { get; set; }
        public IEnumerable<ComplexDetails> ComplexData { get; set; }
        public string FilterBy { get; set; }
        public string SearchTerm { get; set; }
        public string CurrentSort { get; set; }
        public IEnumerable<string> FilterOptions => new List<string> { "Name", "Min GPA", "Min Grad Date" };

    }
}
