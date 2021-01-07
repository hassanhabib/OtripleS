// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(sqlException);

            var badGuid = Guid.NewGuid();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(badGuid))
                    .Throws(sqlException);

            // when 
            ValueTask<Attachment> retrieveTask = 
                this.attachmentService.RetrieveAttachmentByIdAsync(badGuid);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() => retrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(badGuid),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var guid = Guid.NewGuid();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(guid))
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Attachment> retrieveTask = 
                this.attachmentService.RetrieveAttachmentByIdAsync(guid);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() => retrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(guid),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedAttachmentServiceException =
                new AttachmentServiceException(exception);

            var guid = Guid.NewGuid();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(guid))
                    .Throws(exception);

            // when 
            ValueTask<Attachment> retrieveTask = 
                this.attachmentService.RetrieveAttachmentByIdAsync(guid);

            // then
            await Assert.ThrowsAsync<AttachmentServiceException>(() => retrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(guid),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
