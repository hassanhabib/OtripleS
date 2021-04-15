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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomExamFeeId = default;
            Guid inputExamFeeId = randomExamFeeId;

            var invalidExamFeeInputException = new InvalidExamFeeException(
                parameterName: nameof(ExamFee.Id),
                parameterValue: inputExamFeeId);
            var expectedExamFeeValidationException =
                new ExamFeeValidationException(invalidExamFeeInputException);

            //when
            ValueTask<ExamFee> actualExamFeeDeleteTask =
                this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            //then
            await Assert.ThrowsAsync<ExamFeeValidationException>(
                () => actualExamFeeDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageExamFeeIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(randomDateTime);
            Guid inputExamFeeId = randomExamFee.Id;
            ExamFee nullStorageExamFee = null;

            var notFoundExamFeeException = new NotFoundExamFeeException(inputExamFeeId);

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(notFoundExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ReturnsAsync(nullStorageExamFee);

            // when
            ValueTask<ExamFee> actualExamFeeDeleteTask =
                this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() => actualExamFeeDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
