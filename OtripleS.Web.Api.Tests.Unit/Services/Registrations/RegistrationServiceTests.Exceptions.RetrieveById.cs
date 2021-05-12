// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someRegistrationId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedRegistrationDependencyException =
                new RegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Registration> retrieveRegistrationTask =
                this.registrationService.RetrieveRegistrationByIdAsync(someRegistrationId);

            // then
            await Assert.ThrowsAsync<RegistrationDependencyException>(() =>
                retrieveRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedRegistrationDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
