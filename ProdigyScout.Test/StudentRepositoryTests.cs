using ProdigyScout.Models;
using ProdigyScout.Tests.Fixture;
using ProdigyScout.Tests.Helpers;
using ProdigyScout.Interfaces;
using ProdigyScout.ViewModels;
using static ProdigyScout.Helpers.Enums;

namespace ProdigyScout.Tests
{
    [Collection("DisablesParallelExecution")]
    public class StudentRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests(TestDatabaseFixture fixture)
        {
            _fixture = fixture;
            _repository = new StudentRepository(_fixture.CreateContext());
        }

        // Testing GetStudents method
        [Fact]
        public async Task Get_Students_FilterBy_FirstName()
        {
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "John",
                LastName = "Mullaney",
                Gender = "Unidentified",
                EmailID = "john.mullaney@example.com",
                GPA = (float)3.9,
                GraduationDate = DateTime.Now.Date
            };

            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("First Name", "John", "FirstName");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_LastName()
        {
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Frank",
                LastName = "Doe",
                Gender = "Defined",
                EmailID = "frank.doe@example.com",
                GPA = (float)3.8,
                GraduationDate = DateTime.Now.Date
            };

            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Last Name", "Doe", "LastName");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Email()
        {
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Max",
                LastName = "All",
                Gender = "Trans",
                EmailID = "john.doe@example.com",
                GPA = (float)3.7,
                GraduationDate = DateTime.Now.Date
            };
            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Email", "john.doe@example.com", "Email");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Gender()
        {
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Steve",
                LastName = "Jobs",
                Gender = "Male",
                EmailID = "john.doe@example.com",
                GPA = (float)3.6,
                GraduationDate = DateTime.Now.Date
            };
            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Gender", "Male", "Gender");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_GPA()
        {
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Right",
                LastName = "Way",
                Gender = "Female",
                EmailID = "right.foe@example.com",
                GPA = (float)3.35,
                GraduationDate = DateTime.Now.Date
            };
            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("GPA", "3.2", "GPA");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_GraduationDate()
        {
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Lmao",
                LastName = "Test",
                Gender = "Non-binary",
                EmailID = "loe.more@example.com",
                GPA = (float)3.0,
                GraduationDate = DateTime.Now.Date
            };
            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Graduation Date", "2022-06-01", "GraduationDate");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Student_ById()
        {
            // Arrange.
            int studentId = 1;

            // Act.
            Prospect student = await _repository.GetStudentByID(studentId);

            // Assert.
            Assert.Equal(Constants.LAST_NAME_1, student.LastName);
        }

        [Fact]
        public async Task Get_Student_ById_NotFound()
        {
            // Arrange.
            int studentId = -1;

            // Act.
            Prospect student = await _repository.GetStudentByID(studentId);

            // Assert.
            Assert.Null(student);
        }

        [Fact]
        public async Task Get_Student_ById_After_Insert()
        {
            // Arrange.
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Test",
                LastName = "GetById",
                Gender = "Male",
                GPA = 0,
                GraduationDate = DateTime.Now
            };

            // Act.
            Prospect newStudent = await _repository.InsertStudent(viewModel);
            Prospect sudent = await _repository.GetStudentByID(newStudent.Id);

            // Assert.
            Assert.Same(newStudent, sudent);
            Assert.Equal(sudent.LastName, viewModel.LastName);

            // Cleanup.
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Insert_Student()
        {
            // Arrange.
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Test",
                LastName = "Insert",
                Gender = "Male",
                GPA = (float)3.35,
                GraduationDate = DateTime.Now.Date
            };

            // Act.
            Prospect newStudent = await _repository.InsertStudent(viewModel);
            Prospect student = await _repository.GetStudentByID(newStudent.Id);

            // Assert.
            Assert.Same(newStudent, student);
            Assert.Equal(student.LastName, viewModel.LastName);
            Assert.Equal(student.GraduationDate, viewModel.GraduationDate);

            // Cleanup.
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Update_Student()
        {
            // Arrange.
            string tempLastName = "Update_Update";

            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Test",
                LastName = "Update",
                Gender = "Male",
                GPA = (float)3.35,
                GraduationDate = DateTime.Now.Date
            };

            // Act.
            Prospect newStudent = await _repository.InsertStudent(viewModel);

            viewModel.Id = newStudent.Id;
            viewModel.FirstName = newStudent.FirstName;
            viewModel.LastName = tempLastName;
            viewModel.GraduationDate = DateTime.Now.Date;

            Prospect student = await _repository.UpdateStudent(viewModel);

            // Assert.
            Assert.IsAssignableFrom<Prospect>(student);
            Assert.Equal(student.LastName, tempLastName);

            // Cleanup.
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Delete_Student()
        {
            // Arrange.
            StudentViewModel viewModel = new StudentViewModel()
            {
                FirstName = "Test",
                LastName = "Delete",
                Gender = "Male",
                GPA = (float)3.35,
                GraduationDate = DateTime.Now.Date
            };

            // Act.
            Prospect newStudent = await _repository.InsertStudent(viewModel);

            int id = newStudent.Id;
            await _repository.DeleteStudent(id);

            Prospect student = await _repository.GetStudentByID(id);

            // Assert.
            Assert.Null(student);
        }
    }
}
