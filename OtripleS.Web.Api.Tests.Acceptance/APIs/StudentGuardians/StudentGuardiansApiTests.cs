// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using System;
using System.Collections.Generic;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentGuardians
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentGuardiansApiTests    
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentGuardiansApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private StudentGuardian CreateRandomStudentGuardian() =>
            CreateRandomStudentGuardianFiller().Create();

        private IEnumerable<StudentGuardian> GetRandomStudentGuardians() =>
            CreateRandomStudentGuardianFiller().Create(GetRandomNumber());

        private Filler<StudentGuardian> CreateRandomStudentGuardianFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<StudentGuardian>();

            filler.Setup()
                .OnProperty(studentGuardian => studentGuardian.CreatedBy).Use(posterId)
                .OnProperty(studentGuardian => studentGuardian.UpdatedBy).Use(posterId)
                .OnProperty(studentGuardian => studentGuardian.CreatedDate).Use(now)
                .OnProperty(studentGuardian => studentGuardian.UpdatedDate).Use(now)
                .OnProperty(studentGuardian => studentGuardian.Student).IgnoreIt()
                .OnProperty(studentGuardian => studentGuardian.Guardian).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());


            return filler;
        }

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

        private Guardian CreateRandomGuardian() =>
            CreateRandomGuardianFiller().Create();

        private Filler<Guardian> CreateRandomGuardianFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Guardian>();

            filler.Setup()
                .OnProperty(guardian => guardian.CreatedBy).Use(posterId)
                .OnProperty(guardian => guardian.UpdatedBy).Use(posterId)
                .OnProperty(guardian => guardian.CreatedDate).Use(now)
                .OnProperty(guardian => guardian.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(guardian => guardian.StudentGuardians).IgnoreIt();

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 10).GetValue();
    }
}
