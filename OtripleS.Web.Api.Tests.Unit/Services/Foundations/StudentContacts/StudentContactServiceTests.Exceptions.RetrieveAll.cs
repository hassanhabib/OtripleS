// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentContacts())
                .Throws(sqlException);

            // when
            Action retrieveAllStudentContactAction = () =>
                this.studentContactService.RetrieveAllStudentContacts();

            // then
            Assert.Throws<StudentContactDependencyException>(
                retrieveAllStudentContactAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(
                        expectedStudentContactDependencyException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedStudentContactServiceException =
                new StudentContactServiceException(serviceException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentContacts())
                .Throws(serviceException);

            // when
            Action retrieveAllStudentContactAction = () =>
                this.studentContactService.RetrieveAllStudentContacts();

            // then
            Assert.Throws<StudentContactServiceException>(
                retrieveAllStudentContactAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedStudentContactServiceException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
