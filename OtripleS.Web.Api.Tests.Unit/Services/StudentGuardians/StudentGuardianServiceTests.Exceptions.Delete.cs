// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() => deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedStudentGuardianException =
                new LockedStudentGuardianException(databaseUpdateConcurrencyException);

            var expectedStudentGuardianException =
                new StudentGuardianDependencyException(lockedStudentGuardianException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() => deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowServiceExceptionOnDeleteWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid inputStudentId = randomStudentId;
            var exception = new Exception();

            var expectedStudentGuardianException =
                new StudentGuardianServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianServiceException>(() =>
                deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
