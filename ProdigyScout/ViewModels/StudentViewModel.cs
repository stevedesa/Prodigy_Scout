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
            Degree = string.Empty;
            GraduationDate = DateTime.Today;
            ResumePath = string.Empty;
            ImagePath = string.Empty;
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
                Degree = student.Degree;
                ResumePath = student.ResumePath;
                ImagePath = student.ImagePath;
                IsWatched = student.ComplexDetails != null && student.ComplexDetails.IsWatched;
                IsPipeline = student.ComplexDetails != null && student.ComplexDetails.IsPipeline;
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
        [DisplayName("Degree")]
        public string Degree { get; set; }

        [Required]
        [DisplayName("Graduation Date")]
        [DataType(DataType.Date)]
        public DateTime GraduationDate { get; set; }

        [DisplayName("Resume")]
        public IFormFile ResumeFile { get; set; }

        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }

        public string GraduationDateFormatted
        {
            get { return string.Concat(GraduationDate.ToString("MMM"), " ", GraduationDate.ToString("yyyy")); }
        }

        public bool IsWatched { get; set; }
        public bool IsPipeline { get; set; }
        public string Comment { get; set; }
        public string ResumePath { get; set; }
        public string ImagePath { get; set; }

        public IEnumerable<Prospect> Students { get; set; }
        public IEnumerable<ComplexDetails> ComplexData { get; set; }
        public string FilterBy { get; set; }
        public string SearchTerm { get; set; }
        public string CurrentSort { get; set; }
        public IEnumerable<string> FilterOptions => new List<string> { "Name", "Min GPA", "Min Grad Date", "Degree" };
        public IEnumerable<string> SortOptions => new List<string> {
            "Prodigies", "Prospects", "Name [A]", "Name [D]", "GPA [A]", "GPA [D]", "GradDate [A]", "GradDate [D]" 
        };
    }
}
