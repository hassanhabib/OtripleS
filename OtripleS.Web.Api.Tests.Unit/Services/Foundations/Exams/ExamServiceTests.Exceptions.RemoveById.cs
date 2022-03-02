// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid randomExamId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            SqlException sqlException = GetSqlException();

            var failedExamStorageException =
                new FailedExamStorageException(sqlException);

            var expectedExamDependencyException =
                new ExamDependencyException(failedExamStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Exam> deleteExamTask =
                this.examService.RemoveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                deleteExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedExamDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnDeleteIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someExamId = Guid.NewGuid();
            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedExamException = 
                new LockedExamException(databaseUpdateConcurrencyException);

            var expectedExamDependencyValidationException =
                new ExamDependencyValidationException(lockedExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExamId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Exam> deleteExamTask =
                this.examService.RemoveExamByIdAsync(someExamId);

            // then
            await Assert.ThrowsAsync<ExamDependencyValidationException>(() =>
                deleteExamTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExamId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteifDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Guid someExamId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedExamStorageException =
                new FailedExamStorageException(databaseUpdateException);

            var expectedExamDependencyException =
                new ExamDependencyException(failedExamStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExamId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Exam> deleteExamTask =
                this.examService.RemoveExamByIdAsync(someExamId);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                deleteExamTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExamId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someExamId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedExamServiceException =
                new FailedExamServiceException(serviceException);

            var expectedExamServiceException =
                new ExamServiceException(failedExamServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExamId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Exam> deleteExamTask =
                this.examService.RemoveExamByIdAsync(someExamId);

            // then
            await Assert.ThrowsAsync<ExamServiceException>(() =>
                deleteExamTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExamId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}