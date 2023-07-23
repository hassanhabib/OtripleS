// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentId, someGuardianId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentGuardian> retrieveStudentGuardianByIdTask =
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(someStudentId, someGuardianId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                retrieveStudentGuardianByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentGuardianDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentId, someGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentId, someGuardianId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentGuardian> retrieveStudentGuardianByIdTask =
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(someStudentId, someGuardianId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                retrieveStudentGuardianByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGuardianDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentId, someGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            Guid someGuardianId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedStudentGuardianServiceException =
                new FailedStudentGuardianServiceException(serviceException);

            var expectedStudentGuardianServiceException =
                new StudentGuardianServiceException(failedStudentGuardianServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentId, someGuardianId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentGuardian> retrieveStudentGuardianByIdTask =
                this.studentGuardianService.RetrieveStudentGuardianByIdAsync(someStudentId, someGuardianId);

            // then
            await Assert.ThrowsAsync<StudentGuardianServiceException>(() =>
                retrieveStudentGuardianByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGuardianServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(someStudentId, someGuardianId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
