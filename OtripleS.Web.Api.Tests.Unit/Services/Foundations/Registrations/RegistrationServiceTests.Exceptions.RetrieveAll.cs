// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllRegistrationsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRegistrations())
                    .Throws(sqlException);

            // when
            Action retrieveAllRegistrations = () =>
                this.registrationService.RetrieveAllRegistrations();

            // then
            Assert.Throws<RegistrationDependencyException>(
                retrieveAllRegistrations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRegistrations(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllRegistrationsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedRegistrationServiceException =
                new FailedRegistrationServiceException(serviceException);

            var expectedRegistrationServiceException =
                new RegistrationServiceException(failedRegistrationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRegistrations())
                    .Throws(serviceException);

            // when
            Action retrieveAllRegistrations = () =>
                this.registrationService.RetrieveAllRegistrations();

            // then
            Assert.Throws<RegistrationServiceException>(
                retrieveAllRegistrations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRegistrations(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
