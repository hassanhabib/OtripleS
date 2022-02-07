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
            Action retrieveAllStudentRegistrationAction = () =>
                this.studentRegistrationService.RetrieveAllStudentRegistrations();

            // then
            Assert.Throws<StudentRegistrationDependencyException>(
                retrieveAllStudentRegistrationAction);

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
            var exception = new Exception();

            var expectedStudentRegistrationServiceException =
                new StudentRegistrationServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentRegistrations())
                    .Throws(exception);

             // when
            Action retrieveAllStudentRegistrationAction = () =>
                this.studentRegistrationService.RetrieveAllStudentRegistrations();

            // then
            Assert.Throws<StudentRegistrationServiceException>(
                retrieveAllStudentRegistrationAction);

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
