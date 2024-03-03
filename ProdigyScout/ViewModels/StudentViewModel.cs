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
                email = student.email;
                Gender = student.Gender;
                GPA = student.GPA;
                GraduationDate = student.GraduationDate;
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
        public string email { get; set; }

        [Required]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        [Required]
        [DisplayName("GPA")]
        public float GPA { get; set; }

        [Required]
        [DisplayName("Expected Graduation Date")]
        [DataType(DataType.Date)]
        public DateTime GraduationDate { get; set; }

        public string GraduationDateFormatted
        {
            get { return string.Concat(GraduationDate.ToString("MMM"), " ", GraduationDate.ToString("yyyy")); }
        }

        /*public string FullName
        {
            get { return string.Concat(FirstName, " ", LastName); }
        }*/

        // Helper Attributes
        public IEnumerable<Prospect> Students { get; set; }

    }
}
