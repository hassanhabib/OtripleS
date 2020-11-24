// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Teachers
{
    [Collection(nameof(ApiTestCollection))]
    public partial class TeachersApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public TeachersApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private Teacher CreateRandomTeacher() =>
            CreateRandomTeacherFiller().Create();

        private IEnumerable<Teacher> CreateRandomTeachers() =>
            CreateRandomTeacherFiller().Create(GetRandomNumber());

        private Teacher UpdateTeacherRandom(Teacher teacher)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Teacher>();

            filler.Setup()
                .OnProperty(teacher => teacher.Id).Use(teacher.Id)
                .OnProperty(teacher => teacher.CreatedBy).Use(teacher.CreatedBy)
                .OnProperty(teacher => teacher.UpdatedBy).Use(teacher.UpdatedBy)
                .OnProperty(teacher => teacher.CreatedDate).Use(teacher.CreatedDate)
                .OnProperty(teacher => teacher.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<Teacher> PostRandomTeacherAsync()
        {
            Teacher randomTeacher = CreateRandomTeacher();
            await this.otripleSApiBroker.PostTeacherAsync(randomTeacher);

            return randomTeacher;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Filler<Teacher> CreateRandomTeacherFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid userId = Guid.NewGuid();

            var filler = new Filler<Teacher>();

            filler.Setup()
                .OnProperty(teacher => teacher.CreatedBy).Use(userId)
                .OnProperty(teacher => teacher.UpdatedBy).Use(userId)
                .OnProperty(teacher => teacher.CreatedDate).Use(now)
                .OnProperty(teacher => teacher.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
