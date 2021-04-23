// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.GuardianContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianContacts
{
    public partial class GuardianContactServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedGuardianContactDependencyException =
                new GuardianContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllGuardianContacts())
                .Throws(sqlException);

            //when. then
            Assert.Throws<GuardianContactDependencyException>(() =>
                this.guardianContactService.RetrieveAllGuardianContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllGuardianContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedGuardianContactServiceException =
                new GuardianContactServiceException(exception);

            this.storageBrokerMock.Setup(broker => broker.SelectAllGuardianContacts())
                .Throws(exception);

            // when . then
            Assert.Throws<GuardianContactServiceException>(() =>
                this.guardianContactService.RetrieveAllGuardianContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedGuardianContactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllGuardianContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
