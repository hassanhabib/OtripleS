// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentGuardians
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGuardians())
                    .Throws(sqlException);

            // when
            Action retrieveAllStudentGuardiansAction = () =>
                this.studentGuardianService.RetrieveAllStudentGuardians();

            // then
            Assert.Throws<StudentGuardianDependencyException>(
                retrieveAllStudentGuardiansAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGuardians(),
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
            var exception = new Exception();

            var expectedStudentGuardianServiceException =
                new StudentGuardianServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGuardians())
                    .Throws(exception);

            // when
            Action retrieveAllStudentGuardiansAction = () =>
                this.studentGuardianService.RetrieveAllStudentGuardians();

            // then
            Assert.Throws<StudentGuardianServiceException>(
                retrieveAllStudentGuardiansAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGuardians(),
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
