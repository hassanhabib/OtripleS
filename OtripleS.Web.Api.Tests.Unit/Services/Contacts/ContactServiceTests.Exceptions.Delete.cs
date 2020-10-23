// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.Contacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            SqlException sqlException = GetSqlException();

            var expectedContactDependencyException =
                new ContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Contact> deleteContactTask =
                this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            await Assert.ThrowsAsync<ContactDependencyException>(() =>
                deleteContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            var databaseUpdateException = new DbUpdateException();

            var expectedContactDependencyException =
                new ContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Contact> deleteContactTask =
                this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            await Assert.ThrowsAsync<ContactDependencyException>(() =>
                deleteContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedContactException = new LockedContactException(databaseUpdateConcurrencyException);

            var expectedContactDependencyException =
                new ContactDependencyException(lockedContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Contact> deleteContactTask =
                this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            await Assert.ThrowsAsync<ContactDependencyException>(() =>
                deleteContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            var exception = new Exception();

            var expectedContactServiceException =
                new ContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<Contact> deleteContactTask =
                this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            await Assert.ThrowsAsync<ContactServiceException>(() =>
                deleteContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedContactServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(It.IsAny<Contact>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
