﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.ExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllExamFeesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedExamFeeDependencyException =
                new ExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamFees())
                    .Throws(sqlException);

            // when
            Action retrieveAllExamFeesAction = () =>
                this.examFeeService.RetrieveAllExamFees();

            // then
            Assert.Throws<ExamFeeDependencyException>(
                retrieveAllExamFeesAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedExamFeeDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllExamFeesWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedExamFeeServiceException =
                new ExamFeeServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamFees())
                    .Throws(serviceException);

            // when
            Action retrieveAllExamFeesAction = () =>
                this.examFeeService.RetrieveAllExamFees();

            // then
            Assert.Throws<ExamFeeServiceException>(
                retrieveAllExamFeesAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
