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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = inputAttachment.CreatedBy;
            inputAttachment.UpdatedDate = inputAttachment.CreatedDate;
            var sqlException = GetSqlException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(inputAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(inputAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() =>
                createAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(inputAttachment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = inputAttachment.CreatedBy;
            inputAttachment.UpdatedDate = inputAttachment.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(inputAttachment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Attachment> createAttachmentTask =
                this.attachmentService.AddAttachmentAsync(inputAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() =>
                createAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(inputAttachment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(dateTime);
            Attachment inputAttachment = randomAttachment;
            inputAttachment.UpdatedBy = inputAttachment.CreatedBy;
            inputAttachment.UpdatedDate = inputAttachment.CreatedDate;
            var exception = new Exception();

            var expectedAttachmentServiceException =
                new AttachmentServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(inputAttachment))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Attachment> createAttachmentTask =
                 this.attachmentService.AddAttachmentAsync(inputAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentServiceException>(() =>
                createAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(inputAttachment),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
