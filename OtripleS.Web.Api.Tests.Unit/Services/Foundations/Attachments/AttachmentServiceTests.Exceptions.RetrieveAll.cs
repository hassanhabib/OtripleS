// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Attachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Throws(sqlException);

            // when
             Action retrieveAllAttachmentAction = () =>
             this.attachmentService.RetrieveAllAttachments();

            // then
            Assert.Throws<AttachmentDependencyException>(
               retrieveAllAttachmentAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedAttachmentServiceException =
                new AttachmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllAttachmentAction = () =>
                this.attachmentService.RetrieveAllAttachments();

            // then
            Assert.Throws<AttachmentServiceException>(
                retrieveAllAttachmentAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
