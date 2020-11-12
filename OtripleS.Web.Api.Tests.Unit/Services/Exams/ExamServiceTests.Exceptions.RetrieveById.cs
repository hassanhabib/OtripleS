// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomExamId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            var sqlException = GetSqlException();

            var expectedDependencyException =
                new ExamDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Exam> retrieveExamTask =
                this.examService.RetrieveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                retrieveExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomExamId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            var databaseUpdateException = new DbUpdateException();

            var expectedExamDependencyException =
                new ExamDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Exam> retrieveExamTask =
                this.examService.RetrieveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamDependencyException>(() =>
                retrieveExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
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
            Guid randomExamId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            var exception = new Exception();

            var expectedExamServiceException =
                new ExamServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Exam> retrieveExamByIdTask =
                this.examService.RetrieveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamServiceException>(() =>
                retrieveExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
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
