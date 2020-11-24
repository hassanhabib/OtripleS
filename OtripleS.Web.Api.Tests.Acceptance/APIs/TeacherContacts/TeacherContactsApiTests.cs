// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.TeacherContacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.TeacherContacts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class TeacherContactsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public TeacherContactsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static Teacher CreateRandomTeacher() =>
            CreateTeacherFiller().Create();

        private static Contact CreateRandomContact() =>
            CreateContactFiller().Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<TeacherContact> CreateRandomTeacherContactAsync()
        {
            Contact randomContact = await PostRandomContactAsync();
            Teacher randomTeacher = await PostRandomTeacherAsync();
            var filler = new Filler<TeacherContact>();

            filler.Setup()
                .OnProperty(teacherContact => teacherContact.TeacherId).Use(randomTeacher.Id)
                .OnProperty(teacherContact => teacherContact.ContactId).Use(randomContact.Id);

            TeacherContact teacherContact = filler.Create();

            return teacherContact;
        }

        private async ValueTask<TeacherContact> PostRandomTeacherContactAsync()
        {
            TeacherContact randomTeacherContact = await CreateRandomTeacherContactAsync();
            await this.otripleSApiBroker.PostTeacherContactAsync(randomTeacherContact);

            return randomTeacherContact;
        }

        private async ValueTask<TeacherContact> CreateRandomTeacherContactAsync(Teacher teacher)
        {
            Contact randomContact = await PostRandomContactAsync();
            var filler = new Filler<TeacherContact>();

            filler.Setup()
                .OnProperty(teacherContact => teacherContact.TeacherId).Use(teacher.Id)
                .OnProperty(teacherContact => teacherContact.ContactId).Use(randomContact.Id);

            TeacherContact teacherContact = filler.Create();

            return await this.otripleSApiBroker.PostTeacherContactAsync(teacherContact);
        }

        private async ValueTask<Teacher> PostRandomTeacherAsync()
        {
            Teacher randomTeacher = CreateRandomTeacher();

            return await this.otripleSApiBroker.PostTeacherAsync(randomTeacher);
        }

        private async ValueTask<Contact> PostRandomContactAsync()
        {
            Contact randomContact = CreateRandomContact();

            return await this.otripleSApiBroker.PostContactAsync(randomContact);
        }

        private static Filler<Contact> CreateContactFiller()
        {
            Filler<Contact> filler = new Filler<Contact>();
            Guid randomCreatedUpdatedById = Guid.NewGuid();

            filler.Setup()
                .OnProperty(contact => contact.CreatedBy).Use(randomCreatedUpdatedById)
                .OnProperty(contact => contact.UpdatedBy).Use(randomCreatedUpdatedById)
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }

        private async ValueTask<TeacherContact> DeleteTeacherContactAsync(TeacherContact teacherContact)
        {
            TeacherContact deletedTeacherContact =
                await this.otripleSApiBroker.DeleteTeacherContactByIdAsync(
                    teacherContact.TeacherId, teacherContact.ContactId);

            await this.otripleSApiBroker.DeleteContactByIdAsync(teacherContact.ContactId);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(teacherContact.TeacherId);

            return deletedTeacherContact;
        }

        private static Filler<Teacher> CreateTeacherFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Teacher>();

            filler.Setup()
                .OnProperty(teacher => teacher.CreatedBy).Use(posterId)
                .OnProperty(teacher => teacher.UpdatedBy).Use(posterId)
                .OnProperty(teacher => teacher.CreatedDate).Use(now)
                .OnProperty(teacher => teacher.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }
    }
}
