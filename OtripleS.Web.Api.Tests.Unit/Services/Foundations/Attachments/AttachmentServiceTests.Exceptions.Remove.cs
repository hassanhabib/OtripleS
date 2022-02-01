// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() =>
                deleteAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() =>
                deleteAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedAttachmentException = new LockedAttachmentException(databaseUpdateConcurrencyException);

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() =>
                deleteAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            var Serviceexception = new Exception();

            var expectedAttachmentServiceException =
                new AttachmentServiceException(Serviceexception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(someAttachmentId))
                    .ThrowsAsync(Serviceexception);

            // when
            ValueTask<Attachment> deleteAttachmentTask =
                this.attachmentService.RemoveAttachmentByIdAsync(someAttachmentId);

            // then
            await Assert.ThrowsAsync<AttachmentServiceException>(() =>
                deleteAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
