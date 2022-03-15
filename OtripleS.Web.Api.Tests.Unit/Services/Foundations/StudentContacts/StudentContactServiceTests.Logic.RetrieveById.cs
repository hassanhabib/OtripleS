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
        public async Task ShouldRetrieveStudentContactById()
        {
            // given
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentContact randomStudentContact = CreateRandomStudentContact(inputDateTime);
            StudentContact storageStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = storageStudentContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(
                    randomStudentContact.StudentId,
                    randomStudentContact.ContactId))
                        .ReturnsAsync(randomStudentContact);

            // when
            StudentContact actualStudentContact = await
                this.studentContactService.RetrieveStudentContactByIdAsync(
                    randomStudentContact.StudentId, randomStudentContact.ContactId);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(randomStudentContact.StudentId, randomStudentContact.ContactId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
