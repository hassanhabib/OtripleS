// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            SqlException sqlException = GetSqlException();

            var expectedExamFeeDependencyException = new ExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ExamFee> deleteExamFeeTask =
                this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() =>
                deleteExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            var databaseUpdateException = new DbUpdateException();

            var expectedExamFeeDependencyException = new ExamFeeDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ExamFee> deleteExamFeeTask =
                this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() => deleteExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedExamFeeException = new LockedExamFeeException(databaseUpdateConcurrencyException);

            var expectedExamFeeException = new ExamFeeDependencyException(lockedExamFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<ExamFee> deleteExamFeeTask =
                this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() => deleteExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamFeeException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
