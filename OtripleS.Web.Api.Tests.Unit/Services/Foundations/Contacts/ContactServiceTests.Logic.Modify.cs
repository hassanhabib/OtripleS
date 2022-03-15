// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldModifyContactAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Contact randomContact = CreateRandomContact(randomInputDate);
            Contact inputContact = randomContact;
            Contact afterUpdateStorageContact = inputContact;
            Contact expectedContact = afterUpdateStorageContact;
            Contact beforeUpdateStorageContact = randomContact.DeepClone();
            inputContact.UpdatedDate = randomDate;
            Guid contactId = inputContact.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectContactByIdAsync(contactId))
                    .ReturnsAsync(beforeUpdateStorageContact);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateContactAsync(inputContact))
                    .ReturnsAsync(afterUpdateStorageContact);

            // when
            Contact actualContact =
                await this.contactService.ModifyContactAsync(inputContact);

            // then
            actualContact.Should().BeEquivalentTo(expectedContact);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectContactByIdAsync(contactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateContactAsync(inputContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
