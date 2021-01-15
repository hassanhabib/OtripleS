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

        
    }
}