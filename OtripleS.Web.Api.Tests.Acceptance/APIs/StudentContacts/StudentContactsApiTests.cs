// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Contacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentContacts;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentContacts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentContactsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentContactsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static Student CreateRandomStudent() =>
            CreateStudentFiller().Create();

        private static Contact CreateRandomContact() =>
            CreateContactFiller().Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<StudentContact> CreateRandomStudentContactAsync()
        {
            Contact randomContact = await PostRandomContactAsync();
            Student randomStudent = await PostRandomStudentAsync();
            var filler = new Filler<StudentContact>();

            filler.Setup()
                .OnProperty(studentContact => studentContact.StudentId).Use(randomStudent.Id)
                .OnProperty(studentContact => studentContact.ContactId).Use(randomContact.Id);

            StudentContact studentContact = filler.Create();

            return studentContact;
        }

        private async ValueTask<StudentContact> CreateRandomStudentContactAsync(Student student)
        {
            Contact randomContact = await PostRandomContactAsync();
            var filler = new Filler<StudentContact>();

            filler.Setup()
                .OnProperty(studentContact => studentContact.StudentId).Use(student.Id)
                .OnProperty(studentContact => studentContact.ContactId).Use(randomContact.Id);

            StudentContact studentContact = filler.Create();

            return await this.otripleSApiBroker.PostStudentContactAsync(studentContact);
        }

        private async ValueTask<Student> PostRandomStudentAsync()
        {
            Student randomStudent = CreateRandomStudent();

            return await this.otripleSApiBroker.PostStudentAsync(randomStudent);
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

        private async ValueTask DeleteStudentContactAsync(StudentContact studentContact)
        {
            await this.otripleSApiBroker.DeleteStudentContactAsync(
                studentContact.StudentId, studentContact.ContactId);

            await this.otripleSApiBroker.DeleteContactByIdAsync(studentContact.ContactId);
            await this.otripleSApiBroker.DeleteStudentByIdAsync(studentContact.StudentId);
        }

        private static Filler<Student> CreateStudentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.CreatedBy).Use(posterId)
                .OnProperty(student => student.UpdatedBy).Use(posterId)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
        }
    }
}
