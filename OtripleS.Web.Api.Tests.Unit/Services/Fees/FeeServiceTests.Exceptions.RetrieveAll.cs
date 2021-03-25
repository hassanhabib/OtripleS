// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllFeesWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedFeeDependencyException =
                new FeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFees())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<FeeDependencyException>(() =>
                this.feeService.RetrieveAllFees());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedFeeDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllFeesWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedAttachmentDependencyException =
                new FeeDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFees())
                    .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<FeeDependencyException>(() =>
                this.feeService.RetrieveAllFees());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllFeesWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedFeeServiceException =
                new FeeServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFees())
                    .Throws(exception);

            // when . then
            Assert.Throws<FeeServiceException>(() =>
                this.feeService.RetrieveAllFees());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFees(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
