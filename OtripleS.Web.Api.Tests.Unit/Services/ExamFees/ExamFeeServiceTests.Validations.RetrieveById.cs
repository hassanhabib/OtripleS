// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomExamFeeId = default;
            Guid inputExamFeeId = randomExamFeeId;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.Id),
                parameterValue: inputExamFeeId);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            // when
            ValueTask<ExamFee> retrieveExamFeeByIdTask =
                this.examFeeService.RetrieveExamFeeByIdAsync(inputExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                retrieveExamFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageExamFeeIsNullAndLogItAsync()
        {
            // given
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            ExamFee invalidStorageExamFee = null;
            var notFoundExamFeeException = new NotFoundExamFeeException(inputExamFeeId);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(notFoundExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ReturnsAsync(invalidStorageExamFee);

            // when
            ValueTask<ExamFee> retrieveExamFeeByIdTask =
                this.examFeeService.RetrieveExamFeeByIdAsync(inputExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                retrieveExamFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
