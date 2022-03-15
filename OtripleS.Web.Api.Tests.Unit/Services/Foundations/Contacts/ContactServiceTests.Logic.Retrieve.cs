// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveContactById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(dateTime);
            Guid inputContactId = randomContact.Id;
            Contact inputContact = randomContact;
            Contact expectedContact = randomContact;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectContactByIdAsync(inputContactId))
                .ReturnsAsync(inputContact);

            //when 
            Contact actualContact = await this.contactService.RetrieveContactByIdAsync(inputContactId);

            //then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
