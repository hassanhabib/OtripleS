// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.GuardianAttachments;
using OtripleS.Web.Api.Models.GuardianAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someGuardianId = randomGuardianId;
            SqlException sqlException = GetSqlException();

            var expectedGuardianAttachmentDependencyException
                = new GuardianAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianAttachment> removeGuardianAttachmentTask =
                this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(
                    someGuardianId,
                    someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                removeGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someGuardianId = randomGuardianId;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianAttachmentDependencyException =
                new GuardianAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianAttachment> removeGuardianAttachmentTask =
                this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync
                (someGuardianId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(
                () => removeGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            var randomGuardianId = Guid.NewGuid();
            Guid someAttachmentId = randomAttachmentId;
            Guid someGuardianId = randomGuardianId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttachmentException =
                new LockedGuardianAttachmentException(databaseUpdateConcurrencyException);

            var expectedGuardianAttachmentException =
                new GuardianAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<GuardianAttachment> removeGuardianAttachmentTask =
                this.guardianAttachmentService.RemoveGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                removeGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuardianAttachmentAsync(It.IsAny<GuardianAttachment>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}