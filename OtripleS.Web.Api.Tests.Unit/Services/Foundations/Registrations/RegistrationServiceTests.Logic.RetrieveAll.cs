// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllRegistrations()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Registration> randomRegistrations =
                CreateRandomRegistrations(randomDateTime);

            IQueryable<Registration> storageRegistrations =
                randomRegistrations;

            IQueryable<Registration> expectedRegistrations =
                storageRegistrations;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllRegistrations())
                    .Returns(storageRegistrations);

            // when
            IQueryable<Registration> actualRegistrations =
                this.registrationService.RetrieveAllRegistrations();

            // then
            actualRegistrations.Should().BeEquivalentTo(expectedRegistrations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllRegistrations(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
