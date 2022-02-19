// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllAssignmentAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var failedAssigmentAttachmentStorageException =
                new FailedAssignmentAttachmentStorageException(sqlException);

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(failedAssigmentAttachmentStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignmentAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllAssignmentAttachmentsAction = () =>
                this.assignmentAttachmentService.RetrieveAllAssignmentAttachments();

            // then
            Assert.Throws<AssignmentAttachmentDependencyException>(
                retrieveAllAssignmentAttachmentsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignmentAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAssignmentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllAssignmentAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();
            
            var failedAssignmentAttachmentServiceException =
                new FailedAssignmentAttachmentServiceException(serviceException);

            var expectedAssignmentAttachmentServiceException =
                new AssignmentAttachmentServiceException(failedAssignmentAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignmentAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllAssignmentAttachmentsAction = () =>
                this.assignmentAttachmentService.RetrieveAllAssignmentAttachments();

            // then
            Assert.Throws<AssignmentAttachmentServiceException>(
                retrieveAllAssignmentAttachmentsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignmentAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAssignmentAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
