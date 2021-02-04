// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCourseId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedCourseAttachmentDependencyException = 
                new CourseAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync
                    (someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                retrieveCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseAttachmentDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedCourseAttachmentDependencyException =
                new CourseAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CourseAttachment> retrieveAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync
                    (someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                retrieveAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCourseAttachmentException =
                new LockedCourseAttachmentException(databaseUpdateConcurrencyException);

            var expectedCourseAttachmentException =
                new CourseAttachmentDependencyException(lockedCourseAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                retrieveCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCourseId = Guid.NewGuid();
            var exception = new Exception();

            var expectedCourseAttachmentException =
                new CourseAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<CourseAttachment> retrieveCourseAttachmentTask =
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync
                    (someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentServiceException>(() =>
                retrieveCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
