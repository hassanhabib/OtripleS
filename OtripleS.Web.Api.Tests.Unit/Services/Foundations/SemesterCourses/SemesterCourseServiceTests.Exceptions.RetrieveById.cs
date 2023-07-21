// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSemesterCourseId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(someSemesterCourseId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SemesterCourse> retrieveSemesterCourseByIdTask =
                this.semesterCourseService.RetrieveSemesterCourseByIdAsync(someSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() =>
                retrieveSemesterCourseByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSemesterCourseDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(someSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSemesterCourseId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(someSemesterCourseId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<SemesterCourse> retrieveSemesterCourseByIdTask =
                this.semesterCourseService.RetrieveSemesterCourseByIdAsync(someSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() =>
                retrieveSemesterCourseByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(someSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someSemesterCourseId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSemesterCourseServiceException =
                new FailedSemesterCourseServiceException(serviceException);

            var expectedSemesterCourseServiceException =
                new SemesterCourseServiceException(failedSemesterCourseServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(someSemesterCourseId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SemesterCourse> retrieveSemesterCourseByIdTask =
                this.semesterCourseService.RetrieveSemesterCourseByIdAsync(someSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseServiceException>(() =>
                retrieveSemesterCourseByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(someSemesterCourseId),
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
