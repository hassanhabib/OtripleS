// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            SqlException sqlException = GetSqlException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            StudentRegistration someStudentRegistration = CreateRandomStudentRegistration();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedStudentRegistrationException =
                new LockedStudentRegistrationException(databaseUpdateConcurrencyException);

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(lockedStudentRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ReturnsAsync(someStudentRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            StudentRegistration someStudentRegistration = CreateRandomStudentRegistration();
            var serviceException = new Exception();

            var expectedStudentRegistrationException =
                new StudentRegistrationServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .ReturnsAsync(someStudentRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationServiceException>(() =>
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
