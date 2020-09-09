// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Guardian;
using OtripleS.Web.Api.Models.Guardian.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.GuardianServiceTests
{
	public partial class GuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Guardian randomGuardian = CreateRandomGuardian(randomDateTime);
            Guardian someGuardian = randomGuardian;
            someGuardian.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            SqlException sqlException = GetSqlException();

            var expectedGuardianDependencyException =
                new GuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuardianByIdAsync(someGuardian.Id))
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Guardian> modifyGuardianTask =
                this.guardianService.ModifyGuardianAsync(someGuardian);

            // then
            await Assert.ThrowsAsync<GuardianDependencyException>(() =>
                modifyGuardianTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuardianByIdAsync(someGuardian.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGuardianDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
