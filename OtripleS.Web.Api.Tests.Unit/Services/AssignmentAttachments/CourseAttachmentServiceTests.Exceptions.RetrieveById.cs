// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<AssignmentAttachment> retrieveAssignmentAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    someAssignmentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                retrieveAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAssignmentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedAssignmentAttachmentDependencyException =
                new AssignmentAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<AssignmentAttachment> retrieveAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    someAssignmentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                retrieveAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAssignmentAttachmentException =
                new LockedAssignmentAttachmentException(databaseUpdateConcurrencyException);

            var expectedAssignmentAttachmentException =
                new AssignmentAttachmentDependencyException(lockedAssignmentAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<AssignmentAttachment> retrieveAssignmentAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    someAssignmentId, 
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentDependencyException>(() =>
                retrieveAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someAssignmentId = Guid.NewGuid();
            var exception = new Exception();

            var expectedAssignmentAttachmentException =
                new AssignmentAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<AssignmentAttachment> retrieveAssignmentAttachmentTask =
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    someAssignmentId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentServiceException>(() =>
                retrieveAssignmentAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}