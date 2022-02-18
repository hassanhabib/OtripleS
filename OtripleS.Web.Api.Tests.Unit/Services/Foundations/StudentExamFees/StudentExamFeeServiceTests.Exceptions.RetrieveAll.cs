// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllStudentExamFeesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExamFees())
                    .Throws(sqlException);

            // when
            Action retrieveAllStudentExamFeesAction = () =>
                this.studentExamFeeService.RetrieveAllStudentExamFees();

            // then
            Assert.Throws<StudentExamFeeDependencyException>(
                retrieveAllStudentExamFeesAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExamFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentExamFeeDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllStudentExamFeesWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedStudentExamFeeServiceException =
                new FailedStudentExamFeeServiceException(serviceException);

            var expectedStudentExamFeeServiceException =
                new StudentExamFeeServiceException(failedStudentExamFeeServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExamFees())
                    .Throws(serviceException);

            // when
            Action retrieveAllStudentExamFeesAction = () =>
                this.studentExamFeeService.RetrieveAllStudentExamFees();

            // then
            Assert.Throws<StudentExamFeeServiceException>(
                retrieveAllStudentExamFeesAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExamFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentExamFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
