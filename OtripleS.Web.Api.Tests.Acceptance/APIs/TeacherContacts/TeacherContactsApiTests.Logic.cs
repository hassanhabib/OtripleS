// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.TeacherContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.TeacherContacts
{
    public partial class TeacherContactsApiTests
    {
        [Fact]
        public async Task ShouldPostTeacherContactAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            TeacherContact expectedTeacherContact = inputTeacherContact;

            // when 
            await this.otripleSApiBroker.PostTeacherContactAsync(inputTeacherContact);

            TeacherContact actualTeacherContact =
                await this.otripleSApiBroker.GetTeacherContactByIdAsync(
                    inputTeacherContact.TeacherId,
                    inputTeacherContact.ContactId);

            // then
            actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact,
                options => options
                    .Excluding(teacherContact => teacherContact.Teacher)
                    .Excluding(teacherContact => teacherContact.Contact));

            await this.otripleSApiBroker.DeleteTeacherContactByIdAsync(
                actualTeacherContact.TeacherId,
                actualTeacherContact.ContactId);
        }

        [Fact]
        public async Task ShouldGetAllTeachersAsync()
        {
            // given
            IEnumerable<TeacherContact> randomTeacherContacts = CreateRandomTeacherContacts();
            List<TeacherContact> inputTeacherContacts = randomTeacherContacts.ToList();
            List<TeacherContact> expectedTeacherContacts = inputTeacherContacts;

            foreach (TeacherContact inputTeacherContact in inputTeacherContacts)
            {
                await this.otripleSApiBroker.PostTeacherContactAsync(inputTeacherContact);
            }

            // when
            IEnumerable<TeacherContact> actualTeacherContacts =
                await this.otripleSApiBroker.GetAllTeacherContactsAsync();

            // then
            foreach (TeacherContact expectedTeacherContact in expectedTeacherContacts)
            {
                TeacherContact actualTeacherContact =
                    actualTeacherContacts.FirstOrDefault(teacherContact =>
                        teacherContact.TeacherId == expectedTeacherContact.TeacherId
                        && teacherContact.ContactId == expectedTeacherContact.ContactId);

                actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact,
                    options => options
                        .Excluding(teacherContact => teacherContact.Teacher)
                        .Excluding(teacherContact => teacherContact.Contact));

                await this.otripleSApiBroker.DeleteTeacherContactByIdAsync(
                    actualTeacherContact.TeacherId,
                    actualTeacherContact.ContactId);
            }
        }
    }
}