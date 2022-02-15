// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
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
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(
                    someCourseId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCourseAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync
                (someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCourseId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedCourseAttachmentException(databaseUpdateConcurrencyException);

            var expectedCourseAttachmentException =
                new CourseAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(someCourseId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someCourseId = Guid.NewGuid();
            var serviceException = new Exception();
            var expectedCourseAttachmentException = new CourseAttachmentServiceException(serviceException);
            this.storageBrokerMock.Setup(broker =>
             broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId))
                 .ThrowsAsync(serviceException);

            // when
            ValueTask<CourseAttachment> removeCourseAttachmentTask =
                this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(
                    someCourseId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<CourseAttachmentServiceException>(() =>
                removeCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(someCourseId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
