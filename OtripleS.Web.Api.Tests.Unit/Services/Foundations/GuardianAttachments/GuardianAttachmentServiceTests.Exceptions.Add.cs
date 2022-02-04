//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment someGuardianAttachment = randomGuardianAttachment;
            var sqlException = GetSqlException();

            var expectedGuardianAttachmentDependencyException =
                new GuardianAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAttachmentAsync(someGuardianAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(someGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(someGuardianAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment someGuardianAttachment = randomGuardianAttachment;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianAttachmentDependencyException =
                new GuardianAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAttachmentAsync(someGuardianAttachment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                this.guardianAttachmentService.AddGuardianAttachmentAsync(someGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentDependencyException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(someGuardianAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            GuardianAttachment randomGuardianAttachment = CreateRandomGuardianAttachment();
            GuardianAttachment someGuardianAttachment = randomGuardianAttachment;
            var serviceException = new Exception();

            var expectedGuardianAttachmentServiceException =
                new GuardianAttachmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianAttachmentAsync(someGuardianAttachment))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<GuardianAttachment> addGuardianAttachmentTask =
                 this.guardianAttachmentService.AddGuardianAttachmentAsync(someGuardianAttachment);

            // then
            await Assert.ThrowsAsync<GuardianAttachmentServiceException>(() =>
                addGuardianAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianAttachmentAsync(someGuardianAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
