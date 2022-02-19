// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee someExamFee = CreateRandomExamFee(dateTime);
            someExamFee.UpdatedBy = someExamFee.CreatedBy;
            someExamFee.UpdatedDate = someExamFee.CreatedDate;
            var sqlException = GetSqlException();

            var expectedExamFeeDependencyException =
                new ExamFeeDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(someExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() =>
                createExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedExamFeeDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee someExamFee = CreateRandomExamFee(dateTime);
            someExamFee.UpdatedBy = someExamFee.CreatedBy;
            someExamFee.UpdatedDate = someExamFee.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedExamFeeDependencyException =
                new ExamFeeDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                this.examFeeService.AddExamFeeAsync(someExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() =>
                createExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamFeeDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee someExamFee = CreateRandomExamFee(dateTime);
            someExamFee.UpdatedBy = someExamFee.CreatedBy;
            someExamFee.UpdatedDate = someExamFee.CreatedDate;
            var serviceException = new Exception();

            var failedExamFeeServiceException =
                new FailedExamFeeServiceException(serviceException);

            var expectedExamFeeServiceException =
                new ExamFeeServiceException(failedExamFeeServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ExamFee> createExamFeeTask =
                 this.examFeeService.AddExamFeeAsync(someExamFee);

            // then
            await Assert.ThrowsAsync<ExamFeeServiceException>(() =>
                createExamFeeTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(It.IsAny<ExamFee>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamFeeServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
