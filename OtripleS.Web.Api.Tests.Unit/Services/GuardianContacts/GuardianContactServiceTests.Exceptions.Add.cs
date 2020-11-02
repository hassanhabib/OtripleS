//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact inputGuardianContact = randomGuardianContact;
            var sqlException = GetSqlException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianContactAsync(inputGuardianContact))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(inputGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(inputGuardianContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact inputGuardianContact = randomGuardianContact;
            var databaseUpdateException = new DbUpdateException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianContactAsync(inputGuardianContact))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                this.guardianContactService.AddGuardianContactAsync(inputGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactDependencyException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(inputGuardianContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            GuardianContact randomGuardianContact = CreateRandomGuardianContact();
            GuardianContact inputGuardianContact = randomGuardianContact;
            var exception = new Exception();

            var expectedGuardianContactServiceException =
                new GuardianContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuardianContactAsync(inputGuardianContact))
                    .ThrowsAsync(exception);

            // when
            ValueTask<GuardianContact> addGuardianContactTask =
                 this.guardianContactService.AddGuardianContactAsync(inputGuardianContact);

            // then
            await Assert.ThrowsAsync<GuardianContactServiceException>(() =>
                addGuardianContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuardianContactAsync(inputGuardianContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
