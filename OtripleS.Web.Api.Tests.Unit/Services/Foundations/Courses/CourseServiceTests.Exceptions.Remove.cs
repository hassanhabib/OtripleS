// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedCourseDependencyException =
                new CourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(someCourseId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.RemoveCourseAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                deleteCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(someCourseId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedCourseDependencyException =
                new CourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(someCourseId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.RemoveCourseAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                deleteCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(someCourseId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCourseException = new LockedCourseException(databaseUpdateConcurrencyException);

            var expectedCourseDependencyException =
                new CourseDependencyException(lockedCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(someCourseId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.RemoveCourseAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                deleteCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(someCourseId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var serviceException = new Exception();

            var expectedCourseServiceException =
                new CourseServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(someCourseId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.RemoveCourseAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseServiceException>(() =>
                deleteCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(someCourseId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
