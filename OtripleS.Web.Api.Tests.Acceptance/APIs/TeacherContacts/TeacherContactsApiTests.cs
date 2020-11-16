// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using OtripleS.Web.Api.Models.Contacts;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
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

        private static TeacherContact CreateRandomTeacherContact() =>
            CreateTeacherContactFiller().Create();

        private static IEnumerable<TeacherContact> CreateRandomTeacherContacts() =>
            Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(item => CreateRandomTeacherContact());

        private static Teacher CreateRandomTeacher() =>
            CreateTeacherFiller().Create();

        private static Contact CreateRandomContact() =>
            CreateContactFiller().Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static Filler<TeacherContact> CreateTeacherContactFiller()
        {
            Contact randomContact = CreateRandomContact();
            Teacher randomTeacher = CreateRandomTeacher();
            var filler = new Filler<TeacherContact>();

            filler.Setup()
                .OnProperty(teacherContact => teacherContact.Teacher).Use(randomTeacher)
                .OnProperty(teacherContact => teacherContact.TeacherId).Use(randomTeacher.Id)
                .OnProperty(teacherContact => teacherContact.Contact).Use(randomContact)
                .OnProperty(teacherContact => teacherContact.ContactId).Use(randomContact.Id);

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
                .OnProperty(contact => contact.TeacherContacts).IgnoreIt()
                .OnProperty(contact => contact.GuardianContacts).IgnoreIt()
                .OnProperty(contact => contact.UserContacts).IgnoreIt()
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow);

            return filler;
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
                .OnType<DateTimeOffset>().Use(DateTimeOffset.UtcNow)
                .OnProperty(classroom => classroom.SemesterCourses).IgnoreIt()
                .OnProperty(teacher => teacher.TeacherContacts).IgnoreIt()
                .OnProperty(teacher => teacher.ReviewedStudentExams).IgnoreIt();

            return filler;
        }
    }
}
