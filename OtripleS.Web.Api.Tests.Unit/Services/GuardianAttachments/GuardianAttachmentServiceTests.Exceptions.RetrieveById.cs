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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputGuardianId = randomGuardianId;
            SqlException sqlException = GetSqlException();

            var expectedGuardianAttachmentDependencyException
                = new GuardianAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianAttachment> deleteGuardianAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync
                (inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                deleteGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomGuardianId = Guid.NewGuid();
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputGuardianId = randomGuardianId;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianAttachmentDependencyException =
                new GuardianAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianAttachment> deleteAttachmentTask =
                this.guardianAttachmentService.RetrieveGuardianAttachmentByIdAsync
                (inputGuardianId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(
                () => deleteAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianAttachmentByIdAsync(inputGuardianId, inputAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
