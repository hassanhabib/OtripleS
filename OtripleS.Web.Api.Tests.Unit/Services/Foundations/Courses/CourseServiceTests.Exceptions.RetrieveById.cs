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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedCourseDependencyException =
                new CourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Course> retrieveCourseTask =
                this.courseService.RetrieveCourseByIdAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                retrieveCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedCourseDependencyException =
                new CourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Course> retrieveByIdCourseTask =
                this.courseService.RetrieveCourseByIdAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                retrieveByIdCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCourseException = new LockedCourseException(databaseUpdateConcurrencyException);

            var expectedCourseDependencyException =
                new CourseDependencyException(lockedCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Course> retrieveByIdCourseTask =
                this.courseService.RetrieveCourseByIdAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseDependencyException>(() =>
                retrieveByIdCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var exception = new Exception();

            var expectedCourseServiceException =
                new CourseServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Course> retrieveByIdCourseTask =
                this.courseService.RetrieveCourseByIdAsync(someCourseId);

            // then
            await Assert.ThrowsAsync<CourseServiceException>(() =>
                retrieveByIdCourseTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
