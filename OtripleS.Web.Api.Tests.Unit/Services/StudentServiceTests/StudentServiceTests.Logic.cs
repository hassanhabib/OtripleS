using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        public class RegisterAsync : StudentServiceTests
        {
            [Fact]
            public async Task ShouldRegisterNewStudentWhenValidStudent()
            {
                // given
                Student student = CreateRandomStudent();
                storageBrokerMock.Setup(broker => broker.AddStudentAsync(It.IsAny<Student>()))
                    .ReturnsAsync(student);

                // when
                Student registeredStudent = await studentService.RegisterAsync(student);

                // then
                storageBrokerMock.Verify(broker => broker.AddStudentAsync(It.IsAny<Student>()), Times.Once());

                registeredStudent.Should().BeEquivalentTo(student);
            }

            [Fact]
            public async Task ShouldThrowExceptionWhenInvalidStudent()
			{
                // given
                Student student = new Student();

                // when & then
              await Assert.ThrowsAsync<StudentValidationException>(async () => await studentService.RegisterAsync(student));
            }
        }

        [Fact]
        public async Task ShouldDeleteStudentAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            Student randomStudent = CreateRandomStudent();
            Student storageStudent = randomStudent;
            Student expectedStudent = storageStudent;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputId))
                    .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentAsync(storageStudent))
                    .ReturnsAsync(expectedStudent);

            // when
            Student actualStudent =
                await this.studentService.DeleteStudentAsync(inputId);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(storageStudent),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveStudentByIdAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Student randomStudent = CreateRandomStudent();
            Student storageStudent = randomStudent;
            Student expectedStudent = storageStudent;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudentId))
                    .ReturnsAsync(storageStudent);

            // when
            Student actualStudent =
                await this.studentService.RetrieveStudentByIdAsync(inputStudentId);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
