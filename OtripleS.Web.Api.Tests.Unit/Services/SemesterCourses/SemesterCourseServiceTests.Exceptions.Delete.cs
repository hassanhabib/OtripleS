// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            SqlException sqlException = GetSqlException();

            var expectedSemesterCourseDependencyException = new SemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SemesterCourse> deleteSemesterCourseTask =
                this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() =>
                deleteSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            var databaseUpdateException = new DbUpdateException();

            var expectedSemesterCourseDependencyException = new SemesterCourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<SemesterCourse> deleteSemesterCourseTask =
                this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() => deleteSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedSemesterCourseException = new LockedSemesterCourseException(databaseUpdateConcurrencyException);

            var expectedSemesterCourseException = new SemesterCourseDependencyException(lockedSemesterCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<SemesterCourse> deleteSemesterCourseTask =
                this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() => deleteSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            var exception = new Exception();

            var expectedSemesterCourseServiceException = new SemesterCourseServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<SemesterCourse> deleteSemesterCourseTask =
                this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseServiceException>(() =>
                deleteSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}