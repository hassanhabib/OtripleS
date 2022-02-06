// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFeeId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedFeeDependencyException =
                new FeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Fee> removeFeeTask =
                this.feeService.RemoveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeDependencyException>(() =>
                removeFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFeeId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedFeeDependencyException =
                new FeeDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Fee> removeFeeTask =
                this.feeService.RemoveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeDependencyException>(() =>
                removeFeeTask.AsTask());


            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFeeId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedFeeException = new LockedFeeException(databaseUpdateConcurrencyException);

            var expectedFeeDependencyException =
                new FeeDependencyException(lockedFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Fee> removeFeeTask =
                this.feeService.RemoveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeDependencyException>(() =>
                removeFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFeeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFeeId = Guid.NewGuid();
            var serviceException = new Exception();

            var expectedFeeServiceException =
                new FeeServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Fee> removeFeeTask =
                this.feeService.RemoveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeServiceException>(() =>
                removeFeeTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFeeServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
