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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Exam someExam = CreateRandomExam();
            SqlException sqlException = GetSqlException();

            var failedExamStorageException =
                new FailedExamStorageException(sqlException);

            var expectedExamDependencyException =
                new ExamDependencyException(failedExamStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(sqlException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedExamDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Exam someExam = CreateRandomExam();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedExamException =
                new LockedExamException(databaseUpdateConcurrencyException);

            var expectedExamDependencyValidationException =
                new ExamDependencyValidationException(lockedExamException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamDependencyValidationException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamAsync(someExam),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Exam someExam = CreateRandomExam();
            var databaseUpdateException = new DbUpdateException();

            var failedExamStorageException =
                new FailedExamStorageException(databaseUpdateException);

            var expectedExamDependencyException =
                new ExamDependencyException(failedExamStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Exam someExam = CreateRandomExam();
            var serviceException = new Exception();

            var failedExamServiceException =
                new FailedExamServiceException(serviceException);

            var expectedExamServiceException =
                new ExamServiceException(failedExamServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Throws(serviceException);

            // when
            ValueTask<Exam> modifyExamTask =
                this.examService.ModifyExamAsync(someExam);

            // then
            await Assert.ThrowsAsync<ExamServiceException>(() =>
                modifyExamTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExam.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
