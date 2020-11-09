// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.StudentContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentContacts
{
    public partial class StudentContactsApiTests
    {
        [Fact]
        public async Task ShouldGetAllStudentContactAsync()
        {
            // given
            IEnumerable<StudentContact> randomStudentContacts = CreateRandomStudentContacts();
            List<StudentContact> inputStudentContacts = randomStudentContacts.ToList();

            foreach (StudentContact studentContact in inputStudentContacts)
            {
                await this.otripleSApiBroker.PostStudentContactAsync(studentContact);
            }

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

                StudentContact expectedReturnedStudentContact = CreateExpectedStudentContact(expectedStudentContact);

                actualStudentContact.Should().BeEquivalentTo(expectedReturnedStudentContact);
                await this.otripleSApiBroker.DeleteStudentContactAsync(actualStudentContact.StudentId, actualStudentContact.ContactId);
            }
        }

        [Fact]
        public async Task ShouldPostStudentContactAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = inputStudentContact;

            // when 
            await this.otripleSApiBroker.PostStudentContactAsync(inputStudentContact);

            StudentContact actualStudentContact =
                await this.otripleSApiBroker.GetStudentContactAsync(
                    inputStudentContact.StudentId,
                    inputStudentContact.ContactId);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact,
                options => options
                    .Excluding(StudentContact => StudentContact.Student)
                    .Excluding(StudentContact => StudentContact.Contact));

            await this.otripleSApiBroker.DeleteStudentContactAsync(
                actualStudentContact.StudentId,
                actualStudentContact.ContactId);
        }
    }
}
