﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someExamFeeId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedExamFeeDependencyException =
                new ExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(someExamFeeId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ExamFee> retrieveExamFeeByIdTask =
                this.examFeeService.RetrieveExamFeeByIdAsync(someExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() =>
                retrieveExamFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedExamFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(someExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someExamFeeId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedExamFeeDependencyException =
                new ExamFeeDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(someExamFeeId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<ExamFee> retrieveExamFeeByIdTask =
                this.examFeeService.RetrieveExamFeeByIdAsync(someExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeDependencyException>(() =>
                retrieveExamFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(someExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someExamFeeId = Guid.NewGuid();
            var serviceException = new Exception();

            var expectedExamFeeServiceException =
                new ExamFeeServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(someExamFeeId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ExamFee> retrieveExamFeeByIdTask =
                this.examFeeService.RetrieveExamFeeByIdAsync(someExamFeeId);

            // then
            await Assert.ThrowsAsync<ExamFeeServiceException>(() =>
                retrieveExamFeeByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(someExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
