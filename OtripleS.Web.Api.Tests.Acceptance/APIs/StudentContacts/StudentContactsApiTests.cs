// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Force.DeepCloner;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
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

        private static StudentContact CreateRandomStudentContact() =>
            CreateStudentContactFiller().Create();

        private static IEnumerable<StudentContact> CreateRandomStudentContacts() =>
            Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(item => CreateRandomStudentContact());

        private StudentContact CreateExpectedStudentContact(StudentContact studentContact)
        {
            StudentContact expectedStudentContact = studentContact.DeepClone();
            expectedStudentContact.Contact = null;
            expectedStudentContact.Student = null;

            return expectedStudentContact;
        }

        private static Student CreateRandomStudent() =>
            CreateStudentFiller().Create();

        private static Contact CreateRandomContact() =>
            CreateContactFiller().Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Filler<StudentContact> CreateStudentContactFiller()
        {
            Contact randomContact = CreateRandomContact();
            Student randomStudent = CreateRandomStudent();
            var filler = new Filler<StudentContact>();

            filler.Setup()
                .OnProperty(studentContact => studentContact.Student).Use(randomStudent)
                .OnProperty(studentContact => studentContact.StudentId).Use(randomStudent.Id)
                .OnProperty(studentContact => studentContact.Contact).Use(randomContact)
                .OnProperty(studentContact => studentContact.ContactId).Use(randomContact.Id);

            return filler;
        }

        private static Filler<Contact> CreateContactFiller()
        {
            Filler<Contact> filler = new Filler<Contact>();
            Guid randomCreatedUpdatedById = Guid.NewGuid();

            filler.Setup()
                .OnProperty(contact => contact.CreatedBy).Use(randomCreatedUpdatedById)
                .OnProperty(contact => contact.UpdatedBy).Use(randomCreatedUpdatedById)
                .OnProperty(contact => contact.StudentContacts).IgnoreIt()
                .OnProperty(contact => contact.GuardianContacts).IgnoreIt()
                .OnProperty(contact => contact.TeacherContacts).IgnoreIt()
                .OnProperty(contact => contact.UserContacts).IgnoreIt()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
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
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow)
                .OnProperty(student => student.StudentSemesterCourses).IgnoreIt()
                .OnProperty(student => student.StudentGuardians).IgnoreIt()
                .OnProperty(student => student.StudentContacts).IgnoreIt();

            return filler;
        }
    }
}
