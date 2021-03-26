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

namespace OtripleS.Web.Api.Tests.Unit.Services.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
            ValueTask<Fee> retrieveFeeTask =
                this.feeService.RetrieveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeDependencyException>(() =>
                retrieveFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbExceptionOccursAndLogItAsync()
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
            ValueTask<Fee> retrieveByIdFeeTask =
                this.feeService.RetrieveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeDependencyException>(() =>
                retrieveByIdFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveByIdWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someFeeId = Guid.NewGuid();            
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            
            var lockedFeeException = 
                new LockedFeeException(databaseUpdateConcurrencyException);

            var expectedFeeDependencyException =
                new FeeDependencyException(lockedFeeException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Fee> retrieveByIdFeeTask =
                this.feeService.RetrieveFeeByIdAsync(someFeeId);

            // then
            await Assert.ThrowsAsync<FeeDependencyException>(() =>
                retrieveByIdFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
