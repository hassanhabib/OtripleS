//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllTeacherContacts())
                .Throws(sqlException);

            // when
            Action retrieveAllTeacherContactsAction = () =>
                this.teacherContactService.RetrieveAllTeacherContacts();

            // then
            Assert.Throws<TeacherContactDependencyException>(
                retrieveAllTeacherContactsAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(
                        expectedTeacherContactDependencyException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllTeacherContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedTeacherContactServiceException =
                new TeacherContactServiceException(serviceException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllTeacherContacts())
                .Throws(serviceException);

            // when
            Action retrieveAllTeacherContactsAction = () =>
                this.teacherContactService.RetrieveAllTeacherContacts();

            // then
            Assert.Throws<TeacherContactServiceException>(
                retrieveAllTeacherContactsAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedTeacherContactServiceException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllTeacherContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
