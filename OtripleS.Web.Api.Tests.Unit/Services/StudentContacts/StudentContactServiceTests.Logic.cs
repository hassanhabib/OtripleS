//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentStudentContactAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            StudentContact storageStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = storageStudentContact;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentContactAsync(inputStudentContact))
                    .ReturnsAsync(storageStudentContact);

            // when
            StudentContact actualStudentContact =
                await this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(inputStudentContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllStudentContacts()
        {
            //given
            IQueryable<StudentContact> randomSemesterCourses = CreateRandomStudentContacts();
            IQueryable<StudentContact> storageStudentContacts = randomSemesterCourses;
            IQueryable<StudentContact> expectedStudentContacts = storageStudentContacts;

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentContacts())
                .Returns(storageStudentContacts);

            // when
            IQueryable<StudentContact> actualStudentContacts =
                this.studentContactService.RetrieveAllStudentContacts();

            actualStudentContacts.Should().BeEquivalentTo(expectedStudentContacts);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentContacts(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveStudentContactById()
        {
            // given
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentContact randomStudentContact = CreateRandomStudentContact(inputDateTime);
            StudentContact storageStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = storageStudentContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentContactByIdAsync(randomStudentContact.StudentId, randomStudentContact.ContactId))
                    .Returns(new ValueTask<StudentContact>(randomStudentContact));

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
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}