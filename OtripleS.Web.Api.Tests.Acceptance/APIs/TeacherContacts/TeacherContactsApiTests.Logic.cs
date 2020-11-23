// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.TeacherContacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.TeacherContacts
{
    public partial class TeacherContactsApiTests
    {
        [Fact]
        public async Task ShouldPostTeacherContactAsync()
        {
            // given
            TeacherContact randomTeacherContact = await CreateRandomTeacherContactAsync();
            TeacherContact inputTeacherContact = randomTeacherContact;
            TeacherContact expectedTeacherContact = inputTeacherContact;

            // when 
            await this.otripleSApiBroker.PostTeacherContactAsync(inputTeacherContact);

            TeacherContact actualTeacherContact =
                await this.otripleSApiBroker.GetTeacherContactByIdAsync(
                    inputTeacherContact.TeacherId,
                    inputTeacherContact.ContactId);

            // then
            actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact);

            await DeleteTeacherContactAsync(actualTeacherContact);
        }

        [Fact]
        public async Task ShouldGetAllTeacherContactsAsync()
        {
            // given
            Teacher randomTeacher = await PostRandomTeacherAsync();
            List<TeacherContact> randomTeacherContacts = new List<TeacherContact>();

            for (int i = 0; i < GetRandomNumber(); i++)
            {
                randomTeacherContacts.Add(await CreateRandomTeacherContactAsync(randomTeacher));
            }

            List<TeacherContact> inputTeacherContacts = randomTeacherContacts.ToList();
            List<TeacherContact> expectedTeacherContacts = inputTeacherContacts;

            // when
            IEnumerable<TeacherContact> actualTeacherContacts =
                await this.otripleSApiBroker.GetAllTeacherContactsAsync();

            // then
            foreach (TeacherContact expectedTeacherContact in expectedTeacherContacts)
            {
                TeacherContact actualTeacherContact =
                    actualTeacherContacts.Single(teacherContact =>
                        teacherContact.TeacherId == expectedTeacherContact.TeacherId
                        && teacherContact.ContactId == expectedTeacherContact.ContactId);

                actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact);

                await this.otripleSApiBroker.DeleteTeacherContactByIdAsync(
                    actualTeacherContact.TeacherId, actualTeacherContact.ContactId);

                await this.otripleSApiBroker.DeleteContactByIdAsync(actualTeacherContact.ContactId);
            }

            await this.otripleSApiBroker.DeleteTeacherByIdAsync(randomTeacher.Id);
        }
    }
}