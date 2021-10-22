// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedStudentAttachmentDependencyException
                = new StudentAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(
                    someStudentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(() =>
                removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentAttachmentDependencyException =
                new StudentAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync
                (someStudentId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(
                () => removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedStudentAttachmentException(databaseUpdateConcurrencyException);

            var expectedStudentAttachmentException =
                new StudentAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(someStudentId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentDependencyException>(() =>
                removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var exception = new Exception();

            var expectedStudentAttachmentException =
                new StudentAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentAttachment> removeStudentAttachmentTask =
                this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(
                    someStudentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<StudentAttachmentServiceException>(() =>
                removeStudentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(someStudentId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(It.IsAny<StudentAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}