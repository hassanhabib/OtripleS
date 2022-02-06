// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedExamDependencyException =
                new ExamDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExams())
                    .Throws(sqlException);

            // when
            Action retrieveAllexamAction = () =>
                this.examService.RetrieveAllExams();
            
            // then
            Assert.Throws<ExamDependencyException>(
                retrieveAllexamAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedExamDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExams(),
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

            var expectedExamServiceException =
                new ExamServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExams())
                    .Throws(exception);

            // when
            Action retrieveAllExamsAction = () =>
                this.examService.RetrieveAllExams();

            // then
            Assert.Throws<ExamServiceException>(
                retrieveAllExamsAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExams(),
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
