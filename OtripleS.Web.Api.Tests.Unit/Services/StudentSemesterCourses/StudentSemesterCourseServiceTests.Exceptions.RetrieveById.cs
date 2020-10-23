// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentSemesterCourseDependencyException
                = new StudentSemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentSemesterCourse> deleteStudentSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync
                (inputStudentId, inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseDependencyException>(() =>
                deleteStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentSemesterCourseDependencyException =
                new StudentSemesterCourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentSemesterCourse> deleteSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync
                (inputStudentId, inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseDependencyException>(
                () => deleteSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSemesterCourseException =
                new LockedStudentSemesterCourseException(databaseUpdateConcurrencyException);

            var expectedStudentSemesterCourseException =
                new StudentSemesterCourseDependencyException(lockedSemesterCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentSemesterCourse> deleteStudentSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseDependencyException>(() => deleteStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;
            var exception = new Exception();

            var expectedStudentSemesterCourseException =
                new StudentSemesterCourseServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentSemesterCourse> deleteStudentSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync
                (inputStudentId, inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseServiceException>(() =>
                deleteStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
