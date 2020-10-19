// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentContactDependencyException
                = new StudentContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync(
                    someStudentId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someStudentId = randomStudentId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync
                (someStudentId, someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(
                () => removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someStudentId = randomStudentId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedContactException =
                new LockedStudentContactException(databaseUpdateConcurrencyException);

            var expectedStudentContactException =
                new StudentContactDependencyException(lockedContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync(someStudentId, someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid someContactId = randomContactId;
            Guid someStudentId = randomStudentId;
            var exception = new Exception();

            var expectedStudentContactException =
                new StudentContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync(
                    someStudentId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactServiceException>(() =>
                removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}