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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.GuardianAttachments
{
    public partial class GuardianAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedGuardianAttachmentDependencyException
                = new GuardianAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianAttachment> retrieveGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync
                (someGuardianId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                retrieveGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuardianAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianAttachmentDependencyException =
                new GuardianAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianAttachment> retrieveGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync
                (someGuardianId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(
                () => retrieveGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGuardianAttachmentException =
                new LockedGuardianAttachmentException(databaseUpdateConcurrencyException);

            var expectedGuardianAttachmentException =
                new GuardianAttachmentDependencyException(lockedGuardianAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<GuardianAttachment> retrieveGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                retrieveGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var exception = new Exception();

            var expectedGuardianAttachmentException =
                new GuardianAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<GuardianAttachment> retrieveGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync
                (someGuardianId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentServiceException>(() =>
                retrieveGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianAttachmentException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(someGuardianId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
