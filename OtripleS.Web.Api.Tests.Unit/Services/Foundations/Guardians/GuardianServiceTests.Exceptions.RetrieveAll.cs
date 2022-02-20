// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Guardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Guardians
{
    public partial class GuardianServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedGuardianDependencyException =
                new GuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardians())
                    .Throws(sqlException);

            // when
            Action retrieveAllGuardiansAction = () =>
                this.guardianService.RetrieveAllGuardians();

            // then
            Assert.Throws<GuardianDependencyException>(
                retrieveAllGuardiansAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuardianDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardians(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedGuardianServiceException =
                new FailedGuardianServiceException(serviceException);

            var expectedGuardianServiceException =
                new GuardianServiceException(failedGuardianServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuardians())
                    .Throws(serviceException);

            // when
            Action retrieveAllGuardiansAction = () =>
                this.guardianService.RetrieveAllGuardians();

            // then
            Assert.Throws<GuardianServiceException>(
                retrieveAllGuardiansAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuardianServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuardians(),
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
