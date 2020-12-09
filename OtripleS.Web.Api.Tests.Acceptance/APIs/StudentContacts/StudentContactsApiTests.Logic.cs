// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentContacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentContacts
{
    public partial class StudentContactsApiTests
    {
        [Fact]
        public async Task ShouldGetAllStudentContactAsync()
        {
            // given
            Student randomStudent = await PostRandomStudentAsync();
            List<StudentContact> randomStudentContacts = new List<StudentContact>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                randomStudentContacts.Add(await CreateRandomStudentContactAsync(randomStudent));
            }

            List<StudentContact> inputStudentContacts = randomStudentContacts;
            List<StudentContact> expectedStudentContacts = inputStudentContacts;

            // when
            List<StudentContact> actualStudentContacts =
                await this.otripleSApiBroker.GetAllStudentContactsAsync();

            // then
            foreach (StudentContact expectedStudentContact in expectedStudentContacts)
            {
                StudentContact actualStudentContact = actualStudentContacts.Single(
                    studentContact => studentContact.StudentId == expectedStudentContact.StudentId
                    && studentContact.ContactId == expectedStudentContact.ContactId
                    );

                actualStudentContact.Should().BeEquivalentTo(expectedStudentContact);

                await this.otripleSApiBroker.DeleteStudentContactAsync(
                    actualStudentContact.StudentId, actualStudentContact.ContactId);

                await this.otripleSApiBroker.DeleteContactByIdAsync(actualStudentContact.ContactId);
            }

            await this.otripleSApiBroker.DeleteStudentByIdAsync(randomStudent.Id);
        }

        [Fact]
        public async Task ShouldPostStudentContactAsync()
        {
            // given
            StudentContact randomStudentContact = await CreateRandomStudentContactAsync();
            StudentContact inputStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = inputStudentContact;

            // when 
            await this.otripleSApiBroker.PostStudentContactAsync(inputStudentContact);

            StudentContact actualStudentContact =
                await this.otripleSApiBroker.GetStudentContactByIdsAsync(
                    inputStudentContact.StudentId,
                    inputStudentContact.ContactId);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact);

            await DeleteStudentContactAsync(actualStudentContact);
        }

        [Fact]
        public async Task ShouldDeleteStudentContactAsync()
        {
            // given
            StudentContact randomStudentContact = await PostStudentContactAsync();
            StudentContact inputStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = inputStudentContact;

            // when 
            StudentContact deletedStudentContact =
                await DeleteStudentContactAsync(inputStudentContact);

            ValueTask<StudentContact> getStudentContactByIdTask =
                this.otripleSApiBroker.GetStudentContactByIdsAsync(
                    inputStudentContact.StudentId,
                    inputStudentContact.ContactId);

            // then
            deletedStudentContact.Should().BeEquivalentTo(expectedStudentContact);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getStudentContactByIdTask.AsTask());
        }
    }
}
