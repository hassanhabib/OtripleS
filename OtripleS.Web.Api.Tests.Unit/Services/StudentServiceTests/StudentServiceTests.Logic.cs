using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Students;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        public class RegisterAsync : StudentServiceTests
        {
            [Fact]
            public async Task RegistersNewStudent()
            {
                // given
                Student student = CreateRandomStudent();
                storageBrokerMock.Setup(broker => broker.AddStudentAsync(It.IsAny<Student>()))
                    .ReturnsAsync(student);

                // when
                Student registeredStudent = await studentService.RegisterAsync(student.Id, student.FirstName, student.MiddleName, student.LastName, student.BirthDate, student.Gender);

                // then
                storageBrokerMock.Verify(broker => broker.AddStudentAsync(It.IsAny<Student>()), Times.Once());

                Assert.Equal(student.Id, registeredStudent.Id);
                Assert.Equal(student.FirstName, registeredStudent.FirstName);
                Assert.Equal(student.MiddleName, registeredStudent.MiddleName);
                Assert.Equal(student.LastName, registeredStudent.LastName);
                Assert.Equal(student.BirthDate, registeredStudent.BirthDate);
                Assert.Equal(student.Gender, registeredStudent.Gender);
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
    }
}
