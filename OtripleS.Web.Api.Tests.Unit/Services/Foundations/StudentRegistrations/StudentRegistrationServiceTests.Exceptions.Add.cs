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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            StudentRegistration someStudentRegistration = CreateRandomStudentRegistration();
            var sqlException = GetSqlException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(someStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                addStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentRegistrationDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            StudentRegistration someStudentRegistration = CreateRandomStudentRegistration();
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(someStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                addStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            StudentRegistration someStudentRegistration = CreateRandomStudentRegistration();
            var serviceException = new Exception();

            var expectedStudentRegistrationServiceException =
                new StudentRegistrationServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                 this.studentRegistrationService.AddStudentRegistrationAsync(someStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationServiceException>(() =>
                addStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
