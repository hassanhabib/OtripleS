﻿//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedAssigmentAttachmentStorageException =
                new FailedAssignmentAttachmentStorageException(sqlException);

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(failedAssigmentAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                    someAssignmentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedAssigmentAttachmentStorageException =
                new FailedAssignmentAttachmentStorageException(databaseUpdateException);

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(failedAssigmentAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync
                (someAssignmentId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedAssignmentAttachmentException(databaseUpdateConcurrencyException);

            var expectedAssignmentAttachmentDependencyValidationException =
                new AssignmentAttachmentDependencyValidationException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService
                    .RemoveAssignmentAttachmentByIdAsync(
                        assignmentId: someAssignmentId,
                        attachmentId: someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyValidationException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAssignmentAttachmentServiceException =
                new FailedAssignmentAttachmentServiceException(serviceException);

            var expectedAssignmentAttachmentException = 
                new AssignmentAttachmentServiceException(failedAssignmentAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AssignmentAttachment> removeAssignmentAttachmentTask =
                this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                    someAssignmentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentServiceException>(() =>
                removeAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(someAssignmentId, someAttachmentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
