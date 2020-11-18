// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentExamId = Guid.NewGuid();
            Guid inputStudentExamId = randomStudentExamId;
            var sqlException = GetSqlException();

            var expectedStudentExamDependencyException =
                new StudentExamDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentExam> retrieveStudentExamByIdTask =
                this.studentExamService.RetrieveStudentExamByIdAsync(inputStudentExamId);

            // then
            await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
                retrieveStudentExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId),
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
            Guid randomStudentExamId = Guid.NewGuid();
            Guid inputStudentExamId = randomStudentExamId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentExamDependencyException =
                new StudentExamDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentExam> retrieveStudentExamByIdTask =
                this.studentExamService.RetrieveStudentExamByIdAsync(inputStudentExamId);

            // then
            await Assert.ThrowsAsync<StudentExamDependencyException>(() =>
                retrieveStudentExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId),
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
            Guid randomStudentExamId = Guid.NewGuid();
            Guid inputStudentExamId = randomStudentExamId;
            var exception = new Exception();

            var expectedStudentExamServiceException =
                new StudentExamServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentExam> retrieveStudentExamByIdTask =
                this.studentExamService.RetrieveStudentExamByIdAsync(inputStudentExamId);

            // then
            await Assert.ThrowsAsync<StudentExamServiceException>(() =>
                retrieveStudentExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId),
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
