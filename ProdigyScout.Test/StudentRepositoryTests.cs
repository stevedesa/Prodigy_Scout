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

        [Fact]
        public async Task Get_Students_FilterBy_Default()
        {
            // Arrange.

            // Act.
            IList<Prospect> students = await _repository.GetStudents(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            // Assert.
            Assert.Equal(3, students.Count);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName));
        }

        [Fact]
        public async Task Get_Students_FilterBy_None()
        {
            // Arrange.
            var searchString = "qqqxxx";

            // Act.
            IList<Prospect> students = await _repository.GetStudents(searchString, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            // Assert.
            Assert.Equal(0, students.Count);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Many()
        {
            // Arrange.
            var searchString = "Student";

            // Act.
            IList<Prospect> students = await _repository.GetStudents(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            // Assert.
            Assert.Equal(3, students.Count);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName));
        }

        [Fact]
        public async Task Get_Students_FilterBy_Single()
        {
            // Arrange.
            var searchString = Constants.LAST_NAME_1;

            // Act.
            IList<Prospect> students = await _repository.GetStudents(searchString, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            // Assert.
            Assert.Single(students);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName));
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
