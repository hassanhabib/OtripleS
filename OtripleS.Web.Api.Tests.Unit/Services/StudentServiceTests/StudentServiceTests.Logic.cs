using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Requests;
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

        [Fact]
        public async Task ShouldUpdateStudentAsync()
        {
            //given
            Student randomStudent = CreateRandomStudent();
            StudentUpdateDto inputDto = CreateRandomDto();
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent.DeepClone();
            storageStudent.UpdatedDate = DateTimeOffset.Now;
            Student expectedStudent = storageStudent;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectStudentByIdAsync(inputStudent.Id))
                .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                    broker.UpdateStudentAsync(storageStudent))
                .ReturnsAsync(storageStudent);

            // when
            Student actualStudent =
                await this.studentService.ModifyStudentAsync(inputStudent.Id, inputDto);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);
            actualStudent.BirthDate.Should().BeSameDateAs(inputDto.BirthDate);
            actualStudent.CreatedDate.Should().BeSameDateAs(inputStudent.CreatedDate);
            actualStudent.UpdatedDate.Should().BeAfter(inputStudent.UpdatedDate).And.BeBefore(DateTimeOffset.Now);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectStudentByIdAsync(inputStudent.Id),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.UpdateStudentAsync(storageStudent),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}