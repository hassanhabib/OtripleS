// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            SqlException sqlException = GetSqlException();

            var expectedCourseDependencyException =
                new CourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                deleteCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            var databaseUpdateException = new DbUpdateException();

            var expectedCourseDependencyException =
                new CourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                deleteCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCourseException = new LockedCourseException(databaseUpdateConcurrencyException);

            var expectedCourseDependencyException =
                new CourseDependencyException(lockedCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                deleteCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            var exception = new Exception();

            var expectedCourseServiceException =
                new CourseServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Course> deleteCourseTask =
                this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            await Assert.ThrowsAsync<CourseServiceException>(() =>
                deleteCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
