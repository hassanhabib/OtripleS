//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            AssignmentAttachment someAssignmentAttachment = CreateRandomAssignmentAttachment();
            var sqlException = GetSqlException();

            var failedAssigmentAttachmentStorageException =
                new FailedAssignmentAttachmentStorageException(sqlException);

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(failedAssigmentAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(someAssignmentAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(someAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAssignmentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(someAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            AssignmentAttachment someAssignmentAttachment = CreateRandomAssignmentAttachment();
            var databaseUpdateException = new DbUpdateException();

            var failedAssigmentAttachmentStorageException =
                new FailedAssignmentAttachmentStorageException(databaseUpdateException);

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(failedAssigmentAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(someAssignmentAttachment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(someAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(someAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            AssignmentAttachment someAssignmentAttachment = CreateRandomAssignmentAttachment();
            var serviceException = new Exception();

            var failedAssignmentAttachmentServiceException =
                new FailedAssignmentAttachmentServiceException(serviceException);

            var expectedAssignmentAttachmentServiceException =
                new AssignmentAttachmentServiceException(
                    failedAssignmentAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(someAssignmentAttachment))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                 this.assignmentAttachmentService.AddAssignmentAttachmentAsync(someAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentServiceException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(
                    someAssignmentAttachment),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
