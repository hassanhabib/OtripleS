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
