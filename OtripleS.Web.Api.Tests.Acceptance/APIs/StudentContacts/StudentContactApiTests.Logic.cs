using FluentAssertions;
using OtripleS.Web.Api.Models.StudentContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentContacts
{
   public partial class StudentContactApiTests
    {
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
                await this.otripleSApiBroker.GetStudentContactByIdAsync(
                    inputStudentContact.StudentId,
                    inputStudentContact.ContactId);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact,
                options => options
                    .Excluding(StudentContact => StudentContact.Student)
                    .Excluding(StudentContact => StudentContact.Contact));

            await this.otripleSApiBroker.DeleteStudentContactByIdAsync(
                actualStudentContact.StudentId,
                actualStudentContact.ContactId);
        }
    }
}
