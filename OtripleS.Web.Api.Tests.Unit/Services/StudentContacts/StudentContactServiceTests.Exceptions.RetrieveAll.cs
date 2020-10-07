//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
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

            //when. then
            Assert.Throws<StudentContactDependencyException>(() =>
                this.studentContactService.RetrieveAllStudentContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentContacts())
                .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<StudentContactDependencyException>(() =>
                this.studentContactService.RetrieveAllStudentContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
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
            var exception = new Exception();

            var expectedStudentContactServiceException =
                new StudentContactServiceException(exception);

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentContacts())
                .Throws(exception);

            // when . then
            Assert.Throws<StudentContactServiceException>(() =>
                this.studentContactService.RetrieveAllStudentContacts());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedStudentContactServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentContacts(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
