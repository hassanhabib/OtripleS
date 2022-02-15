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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedStudentContactDependencyException
                = new StudentContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(
                    someStudentId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync
                (someStudentId, someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(
                () => retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedContactException =
                new LockedStudentContactException(databaseUpdateConcurrencyException);

            var expectedStudentContactException =
                new StudentContactDependencyException(lockedContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(someStudentId, someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentContactException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var serviceException = new Exception();

            var expectedStudentContactException =
                new StudentContactServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentContact> retrieveStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(
                    someStudentId,
                    someContactId);

            // then
            await Assert.ThrowsAsync<StudentContactServiceException>(() =>
                retrieveStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentContactException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(someStudentId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}