using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ProdigyScout.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProdigyScout.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel() 
        { 
            //Empty Constructor
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

        /*public string FullName
        {
            get { return string.Concat(FirstName, " ", LastName); }
        }*/

        // Helper Attributes
        public IEnumerable<Prospect> Students { get; set; }

    }
}
