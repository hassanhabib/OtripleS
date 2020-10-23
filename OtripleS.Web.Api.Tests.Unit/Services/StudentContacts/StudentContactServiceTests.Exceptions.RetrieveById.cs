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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var inputContactId = randomContactId;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentContactDependencyException
                = new StudentContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(
                    inputStudentId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync
                (inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(
                () => retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedContactException =
                new LockedStudentContactException(databaseUpdateConcurrencyException);

            var expectedStudentContactException =
                new StudentContactDependencyException(lockedContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomStudentId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputStudentId = randomStudentId;
            var exception = new Exception();

            var expectedStudentContactException =
                new StudentContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(
                    inputStudentId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactServiceException>(() =>
                retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}