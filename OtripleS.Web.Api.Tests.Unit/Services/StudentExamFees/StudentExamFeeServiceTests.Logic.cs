// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee();
            randomStudentExamFee.UpdatedBy = randomStudentExamFee.CreatedBy;
            randomStudentExamFee.UpdatedDate = randomStudentExamFee.CreatedDate;
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = randomStudentExamFee;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee))
                    .ReturnsAsync(storageStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllStudentExamFees()
        {
            // given
            IQueryable<StudentExamFee> randomStudentExamFees =
                CreateRandomStudentExamFees();

            IQueryable<StudentExamFee> storageStudentExamFees =
                randomStudentExamFees;

            IQueryable<StudentExamFee> expectedStudentExamFees =
                storageStudentExamFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExamFees())
                    .Returns(storageStudentExamFees);

            // when
            IQueryable<StudentExamFee> actualStudentExamFees =
                this.studentExamFeeService.RetrieveAllStudentExamFees();

            // then
            actualStudentExamFees.Should().BeEquivalentTo(expectedStudentExamFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExamFees(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyStudentExamFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            
            StudentExamFee randomStudentExamFee = 
                CreateRandomStudentExamFee(randomInputDate);
            
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee afterUpdateStorageStudentExamFee = inputStudentExamFee;
            StudentExamFee expectedStudentExamFee = afterUpdateStorageStudentExamFee;
            
            StudentExamFee beforeUpdateStorageStudentExamFee = 
                randomStudentExamFee.DeepClone();
            
            inputStudentExamFee.UpdatedDate = randomDate;
            Guid studentId = inputStudentExamFee.StudentId;
            Guid guardianId = inputStudentExamFee.ExamFeeId;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFee.Id))
                    .ReturnsAsync(beforeUpdateStorageStudentExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentExamFeeAsync(inputStudentExamFee))
                    .ReturnsAsync(afterUpdateStorageStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.ModifyStudentExamFeeAsync(
                    inputStudentExamFee);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFee.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRemoveStudentExamFeeAsync()
        {
            // given
            var randomStudentExamFeeId = Guid.NewGuid();
            Guid inputStudentExamFeeId = randomStudentExamFeeId;
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee();
            randomStudentExamFee.Id = inputStudentExamFeeId;
            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = storageStudentExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId))
                    .ReturnsAsync(storageStudentExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee))
                    .ReturnsAsync(expectedStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
