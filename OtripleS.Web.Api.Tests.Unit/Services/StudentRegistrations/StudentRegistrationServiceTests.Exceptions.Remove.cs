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

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
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
                 broker.SelectStudentRegistrationByIdAsync(inputRegistrationId, inputStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputRegistrationId, 
                    inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputRegistrationId, inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentRegistrationDependencyException))),
                    Times.Once);
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
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
                broker.SelectStudentRegistrationByIdAsync(inputRegistrationId, inputStudentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputRegistrationId, 
                    inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() => 
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputRegistrationId, inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given            
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomStudentRegistrationId = Guid.NewGuid();
            Guid inputStudentRegistrationId = randomStudentRegistrationId;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedStudentRegistrationException =
                new LockedStudentRegistrationException(databaseUpdateConcurrencyException);

            var expectedStudentRegistrationException =
                new StudentRegistrationDependencyException(lockedStudentRegistrationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentRegistrationId, inputStudentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentRegistrationId, 
                    inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() => 
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputStudentRegistrationId, inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
