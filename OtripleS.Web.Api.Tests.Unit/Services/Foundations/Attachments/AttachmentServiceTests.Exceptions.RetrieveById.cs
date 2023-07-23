// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var someAttachmentId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(sqlException);

            // when 
            ValueTask<Attachment> retrieveTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() => retrieveTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogIt()
        {
            // given
            var someAttachmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Attachment> retrieveTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() => retrieveTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogIt()
        {
            // given
            var someAttachmentId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAttachmentServiceException =
                new FailedAttachmentServiceException(serviceException);

            var expectedAttachmentServiceException =
                new AttachmentServiceException(failedAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()))
                    .Throws(serviceException);

            // when 
            ValueTask<Attachment> retrieveTask =
                this.attachmentService.RetrieveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentServiceException>(() => retrieveTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttachmentServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
