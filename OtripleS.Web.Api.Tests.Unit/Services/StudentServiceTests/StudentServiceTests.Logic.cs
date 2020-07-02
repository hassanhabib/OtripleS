using System;
using System.Threading.Tasks;
using Bogus.Extensions;
using FluentAssertions;
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
        public async Task ShouldUpdateStudentAsync()
        {
            //given

            Student randomStudent = CreateRandomStudent();
            StudentUpdateDto inputDto = CreateRandomDto();
            Student storageStudent = randomStudent;

            DateTimeOffset lastUpdatedAt = storageStudent.UpdatedDate;
            
            Student expectedStudent = storageStudent;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectStudentByIdAsync(randomStudent.Id))
                .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                    broker.UpdateStudentAsync(storageStudent))
                .ReturnsAsync(storageStudent);

            // when
            Student actualStudent =
                await this.studentService.ModifyStudentAsync(storageStudent.Id, inputDto);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            actualStudent.BirthDate.Should().BeSameDateAs(inputDto.BirthDate);
            actualStudent.CreatedDate.Should().BeSameDateAs(storageStudent.CreatedDate);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectStudentByIdAsync(storageStudent.Id),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.UpdateStudentAsync(storageStudent),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}