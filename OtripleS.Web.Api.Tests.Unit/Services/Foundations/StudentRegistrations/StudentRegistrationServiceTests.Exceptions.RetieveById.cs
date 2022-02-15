// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            var sqlException = GetSqlException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentRegistration> retrieveStudentRegistrationByIdTask =
                this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                retrieveStudentRegistrationByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
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
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentRegistration> retrieveStudentRegistrationByIdTask =
                this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                retrieveStudentRegistrationByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            var serviceException = new Exception();

            var expectedStudentRegistrationServiceException =
                new StudentRegistrationServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentRegistration> retrieveStudentRegistrationByIdTask =
                this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(
                    inputStudentId,
                    inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationServiceException>(() =>
                retrieveStudentRegistrationByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentId, inputRegistrationId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

