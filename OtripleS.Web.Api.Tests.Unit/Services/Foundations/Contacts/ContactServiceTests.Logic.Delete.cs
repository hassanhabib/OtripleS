// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Contacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Contacts
{
    public partial class ContactServiceTests
    {
        [Fact]
        public async Task ShouldDeleteContactByIdAsync()
        {
            // given
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Contact randomContact = CreateRandomContact(dateTime);
            Contact inputContact = randomContact;
            Guid inputContactId = inputContact.Id;
            inputContact.UpdatedBy = inputContact.CreatedBy;
            inputContact.UpdatedDate = inputContact.CreatedDate;
            Contact storageContact = inputContact;
            Contact expectedContact = inputContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(inputContactId))
                    .ReturnsAsync(inputContact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteContactAsync(inputContact))
                    .ReturnsAsync(storageContact);

            // when
            Contact actualContact =
                await this.contactService.RemoveContactByIdAsync(inputContactId);

            // then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteContactAsync(inputContact),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
