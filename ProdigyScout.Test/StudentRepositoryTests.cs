using ProdigyScout.Models;
using ProdigyScout.Tests.Fixture;
using ProdigyScout.Tests.Helpers;
using ProdigyScout.ViewModels;
using static ProdigyScout.Helpers.Enums;
using ProdigyScout.Repository;

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
            var students = await _repository.GetStudents("Name", "John", "FirstName");
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
            var students = await _repository.GetStudents("Name", "Doe", "LastName");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Name_3()
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
            var students = await _repository.GetStudents("Name", "Max", "Name");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Name_4()
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
            var students = await _repository.GetStudents("Name", "Jobs", "Name");
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
                GPA = (float)3.99,
                GraduationDate = DateTime.Now.Date
            };
            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Min GPA", "3.92", "Min GPA");
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
                GraduationDate = new DateTime(2026, 6, 1)
            };
            var insertedStudent = await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Min Grad Date", "2025-06-01", "GraduationDate");
            Assert.Single(students);
            await _repository.DeleteStudent(insertedStudent.Id);
        }

        [Fact]
        public async Task GetStudent_ById()
        {
            // Arrange
            int studentId = 1;

            // Act
            Prospect student = await _repository.GetStudentByID(studentId);

            // Assert
            Assert.Equal(Constants.LAST_NAME_1, student.LastName);
        }

        [Fact]
        public async Task GetStudent_ById_NotFound()
        {
            // Arrange
            int studentId = -1;

            // Act
            Prospect student = await _repository.GetStudentByID(studentId);

            // Assert
            Assert.Null(student);
        }

        [Fact]
        public async Task GetStudent_ById_After_Insert()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Test",
                LastName = "GetById",
                Gender = "Male",
                GPA = 0,
                GraduationDate = DateTime.Now
            };

            // Act
            Prospect newStudent = await _repository.InsertStudent(viewModel);
            Prospect student = await _repository.GetStudentByID(newStudent.Id);

            // Assert
            Assert.Same(newStudent, student);
            Assert.Equal(student.LastName, viewModel.LastName);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Insert_Student()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Test",
                LastName = "Insert",
                Gender = "Male",
                GPA = 3.35f,
                GraduationDate = DateTime.Now.Date
            };

            // Act
            Prospect newStudent = await _repository.InsertStudent(viewModel);
            Prospect student = await _repository.GetStudentByID(newStudent.Id);

            // Assert
            Assert.Same(newStudent, student);
            Assert.Equal(student.LastName, viewModel.LastName);
            Assert.Equal(student.GraduationDate, viewModel.GraduationDate);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Update_Student()
        {
            // Arrange
            string tempLastName = "Update_Update";

            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Test",
                LastName = "Update",
                Gender = "Male",
                GPA = 3.35f,
                GraduationDate = DateTime.Now.Date
            };

            // Act
            Prospect newStudent = await _repository.InsertStudent(viewModel);

            viewModel.Id = newStudent.Id;
            viewModel.FirstName = newStudent.FirstName;
            viewModel.LastName = tempLastName;
            viewModel.GraduationDate = DateTime.Now.Date;

            Prospect student = await _repository.UpdateStudent(viewModel);

            // Assert
            Assert.IsAssignableFrom<Prospect>(student);
            Assert.Equal(student.LastName, tempLastName);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Delete_Student()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Test",
                LastName = "Delete",
                Gender = "Male",
                GPA = 3.35f,
                GraduationDate = DateTime.Now.Date
            };

            // Act
            Prospect newStudent = await _repository.InsertStudent(viewModel);

            int id = newStudent.Id;
            await _repository.DeleteStudent(id);

            Prospect student = await _repository.GetStudentByID(id);

            // Assert
            Assert.Null(student);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Watched()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Watched",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date,
                IsWatched = true
            };

            // Act
            await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Prodigies", "", "Prodigies");

            // Assert
            Assert.Single(students);
        }

        [Fact]
        public async Task Get_Students_FilterBy_Pipeline()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Pipeline",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date,
                IsPipeline = true
            };

            // Act
            await _repository.InsertStudent(viewModel);
            var students = await _repository.GetStudents("Prospects", "", "Prospects");

            // Assert
            Assert.Single(students);
        }

        [Fact]
        public async Task Insert_Student_With_Resume_And_ImagePath()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Test",
                LastName = "With_Resume",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date,
                ResumePath = "/resumes/example",
                ImagePath = "/profiles/M1.jpg"
            };

            // Act
            var newStudent = await _repository.InsertStudent(viewModel);

            // Assert
            Assert.Equal(viewModel.ResumePath, newStudent.ResumePath);
            Assert.Equal(viewModel.ImagePath, newStudent.ImagePath);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Update_Student_With_Complex_Details()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Update_Complex",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date,
                IsWatched = true,
                IsPipeline = true,
                Comment = "Updated Comment"
            };

            Prospect newStudent = await _repository.InsertStudent(viewModel);
            // Assert
            Assert.True(newStudent.ComplexDetails.IsWatched);
            Assert.True(newStudent.ComplexDetails.IsPipeline);
            Assert.Equal(newStudent.ComplexDetails.Comment, newStudent.ComplexDetails.Comment);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Delete_Student_With_Complex_Details()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Delete_Complex",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date
            };

            var newStudent = await _repository.InsertStudent(viewModel);
            var studentId = newStudent.Id;

            // Act
            await _repository.DeleteStudent(studentId);
            var deletedStudent = await _repository.GetStudentByID(studentId);

            // Assert
            Assert.Null(deletedStudent);
        }

        [Fact]
        public async Task Mark_As_Watch()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Mark_As_Watch",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date
            };

            var newStudent = await _repository.InsertStudent(viewModel);

            // Act
            var result = await _repository.MarkAsWatch(newStudent.Id);
            var watchedStudent = await _repository.GetStudentByID(newStudent.Id);

            // Assert
            Assert.True(result);
            Assert.True(watchedStudent.ComplexDetails.IsWatched);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Mark_As_Pipeline()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Mark_As_Pipeline",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date
            };

            var newStudent = await _repository.InsertStudent(viewModel);

            // Act
            var result = await _repository.MarkAsPipeline(newStudent.Id);
            var pipelinedStudent = await _repository.GetStudentByID(newStudent.Id);

            // Assert
            Assert.True(result);
            Assert.True(pipelinedStudent.ComplexDetails.IsPipeline);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Add_Or_Update_Comment()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Add_Or_Update_Comment",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date
            };

            var newStudent = await _repository.InsertStudent(viewModel);
            var studentId = newStudent.Id;
            var comment = "Test Comment";

            // Act
            var result = await _repository.AddOrUpdateComment(studentId, comment);
            var updatedStudent = await _repository.GetStudentByID(studentId);

            // Assert
            Assert.True(result);
            Assert.Equal(comment, updatedStudent.ComplexDetails.Comment);

            // Cleanup
            await _repository.DeleteStudent(studentId);
        }

        [Fact]
        public async Task Get_Student_By_Email()
        {
            // Arrange
            StudentViewModel viewModel = new StudentViewModel
            {
                FirstName = "Get_Student_By_Email",
                LastName = "Test",
                Gender = "Male",
                GPA = 3.5f,
                GraduationDate = DateTime.Now.Date,
                EmailID = "test@example.com"
            };

            var newStudent = await _repository.InsertStudent(viewModel);

            // Act
            var studentByEmail = await _repository.GetStudentByEmail("test@example.com");

            // Assert
            Assert.NotNull(studentByEmail);
            Assert.Equal(newStudent.Id, studentByEmail.Id);

            // Cleanup
            await _repository.DeleteStudent(newStudent.Id);
        }
    }
}
