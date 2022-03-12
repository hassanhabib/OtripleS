﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveRegistrationByIdAsync()
        {
            // given
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(randomDateTime);
            Registration storageRegistration = randomRegistration;
            Registration expectedRegistration = storageRegistration;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistrationId))
                    .ReturnsAsync(storageRegistration);

            // when
            Registration actualRegistration =
                await this.registrationService.RetrieveRegistrationByIdAsync(inputRegistrationId);

            // then
            actualRegistration.Should().BeEquivalentTo(expectedRegistration);

            this.storageBrokerMock.Verify(broker =>
                 broker.SelectRegistrationByIdAsync(inputRegistrationId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
