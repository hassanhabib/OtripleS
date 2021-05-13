// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenRegistrationsWasEmptyAndLogIt()
        {
            // given
            IQueryable<Registration> emptyStorageRegistrations = new List<Registration>().AsQueryable();
            IQueryable<Registration> expectedRegistrations = emptyStorageRegistrations;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRegistrations())
                    .Returns(expectedRegistrations);

            // when
            IQueryable<Registration> actualRegistrations =
                this.registrationService.RetrieveAllRegistrations();

            // then
            actualRegistrations.Should().BeEquivalentTo(emptyStorageRegistrations);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No Registrations found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRegistrations(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
