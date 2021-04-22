// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
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
                await this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

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

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStudentExamFeeIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentExamFeeId = default;
            Guid inputStudentExamFeeId = randomStudentExamFeeId;

            var invalidStudentExamFeeInputException = new InvalidStudentExamFeeException(
                parameterName: nameof(StudentExamFee.Id),
                parameterValue: inputStudentExamFeeId);

            var expectedStudentExamFeeValidationException =
                new StudentExamFeeValidationException(invalidStudentExamFeeInputException);

            // when
            ValueTask<StudentExamFee> removeStudentExamFeeTask =
                this.StudentExamFeeService.RemoveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeValidationException>(() =>
                removeStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
