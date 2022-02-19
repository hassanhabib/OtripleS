// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentGuardianId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.RemoveStudentGuardianByIdsAsync(someStudentGuardianId, someStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentGuardianDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentGuardianId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.RemoveStudentGuardianByIdsAsync(someStudentGuardianId, someStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() => deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGuardianDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentGuardianId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedStudentGuardianException =
                new LockedStudentGuardianException(databaseUpdateConcurrencyException);

            var expectedStudentGuardianException =
                new StudentGuardianDependencyException(lockedStudentGuardianException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.RemoveStudentGuardianByIdsAsync(someStudentGuardianId, someStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() => deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGuardianException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentGuardianId = Guid.NewGuid();
            Guid someStudentId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedStudentGuardianServiceException =
                new FailedStudentGuardianServiceException(serviceException);

            var expectedStudentGuardianServiceException =
                new StudentGuardianServiceException(failedStudentGuardianServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.RemoveStudentGuardianByIdsAsync(someStudentGuardianId, someStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianServiceException>(() =>
                deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGuardianServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentGuardianId, someStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
