// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldRemoveStudentContactAsync()
        {
            // given
            var randomStudentId = Guid.NewGuid();
            var randomContactId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputContactId = randomContactId;
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentContact randomStudentContact = CreateRandomStudentContact(inputDateTime);
            randomStudentContact.StudentId = inputStudentId;
            randomStudentContact.ContactId = inputContactId;
            StudentContact storageStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = storageStudentContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ReturnsAsync(storageStudentContact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentContactAsync(storageStudentContact))
                    .ReturnsAsync(expectedStudentContact);

            // when
            StudentContact actualStudentContact =
                await this.studentContactService.RemoveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(storageStudentContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}