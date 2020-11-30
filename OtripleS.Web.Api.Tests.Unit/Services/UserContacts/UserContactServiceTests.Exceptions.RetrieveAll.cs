// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Models.UserContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserContacts
{
    public partial class UserContactServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedUserContactDependencyException =
                new UserContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllUserContacts())
                .Throws(sqlException);

            //when. then
            Assert.Throws<UserContactDependencyException>(() =>
                this.userContactService.RetrieveAllUserContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllUserContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedUserContactDependencyException =
                new UserContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllUserContacts())
                .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<UserContactDependencyException>(() =>
                this.userContactService.RetrieveAllUserContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedUserContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllUserContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedUserContactServiceException =
                new UserContactServiceException(exception);

            this.storageBrokerMock.Setup(broker => broker.SelectAllUserContacts())
                .Throws(exception);

            // when . then
            Assert.Throws<UserContactServiceException>(() =>
                this.userContactService.RetrieveAllUserContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedUserContactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllUserContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
