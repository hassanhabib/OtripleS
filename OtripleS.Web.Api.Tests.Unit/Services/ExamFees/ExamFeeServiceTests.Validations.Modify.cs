// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenExamFeeIsNullAndLogItAsync()
        {
            //given
            ExamFee invalidExamFee = null;
            var nullExamFeeException = new NullExamFeeException();

            var expectedExamFeeValidationException =
                new ExamFeeValidationException(nullExamFeeException);

            //when
            ValueTask<ExamFee> modifyExamFeeTask =
                this.examFeeService.ModifyExamFeeAsync(invalidExamFee);

            //then
            await Assert.ThrowsAsync<ExamFeeValidationException>(() =>
                modifyExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
