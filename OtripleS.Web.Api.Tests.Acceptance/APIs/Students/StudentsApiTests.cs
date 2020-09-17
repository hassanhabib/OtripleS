// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Migrations;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using System;
using System.Collections.Generic;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Students
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentsApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private IEnumerable<Student> GetRandomStudents() =>
            CreateRandomStudentFiller().Create(GetRandomNumber());

        private Student CreateRandomStudent() =>
            CreateRandomStudentFiller().Create();

        private Filler<Student> CreateRandomStudentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.CreatedBy).Use(posterId)
                .OnProperty(student => student.UpdatedBy).Use(posterId)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnProperty(student => student.StudentSemesterCourses).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(student => student.StudentGuardians).IgnoreIt();

            return filler;
        }

        private Student UpdateStudentRandom(Student student)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.Id).Use(student.Id)
                .OnProperty(student => student.CreatedBy).Use(student.CreatedBy)
                .OnProperty(student => student.UpdatedBy).Use(student.UpdatedBy)
                .OnProperty(student => student.CreatedDate).Use(student.CreatedDate)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnProperty(student => student.StudentSemesterCourses).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnType<StudentGuardian>().IgnoreIt()
                .OnProperty(student => student.StudentGuardians).IgnoreIt(); ;

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
