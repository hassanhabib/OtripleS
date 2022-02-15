// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllStudentRegistrationsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentRegistrations())
                    .Throws(sqlException);

            // when
            Action retrieveAllStudentRegistrationsAction = () =>
                this.studentRegistrationService.RetrieveAllStudentRegistrations();

            // then
            Assert.Throws<StudentRegistrationDependencyException>(
                retrieveAllStudentRegistrationsAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentRegistrations(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllStudentRegistrationsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedStudentRegistrationServiceException =
                new StudentRegistrationServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentRegistrations())
                    .Throws(serviceException);

            // when
            Action retrieveAllStudentRegistrationsAction = () =>
                this.studentRegistrationService.RetrieveAllStudentRegistrations();

            // then
            Assert.Throws<StudentRegistrationServiceException>(
                retrieveAllStudentRegistrationsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentRegistrations(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentRegistrationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
