// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using System;
using System.Collections.Generic;
using System.Text;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Classrooms
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ClassroomApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;
        public ClassroomApiTests(OtripleSApiBroker otripleSApiBroker)
        {
            this.otripleSApiBroker = otripleSApiBroker;
        }

        private static int GetRandomNumber() => 
            new IntRange(min: 2, max: 10).GetValue();

        private IEnumerable<Classroom> GetRandomClassrooms() =>
            CreateRandumClassroomFiller().Create(GetRandomNumber());

        private Classroom CreateRandumClassroom() =>
            CreateRandumClassroomFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Filler<Classroom> CreateRandumClassroomFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.CreatedBy).Use(posterId)
                .OnProperty(classroom => classroom.UpdatedBy).Use(posterId)
                .OnProperty(classroom => classroom.CreatedDate).Use(now)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private Classroom UpdateClassroomRandom(Classroom classroom)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow; 
            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.Id).Use(classroom.Id)
                .OnProperty(classroom => classroom.CreatedBy).Use(classroom.CreatedBy)
                .OnProperty(classroom => classroom.UpdatedBy).Use(classroom.UpdatedBy)
                .OnProperty(classroom => classroom.CreatedDate).Use(classroom.CreatedDate)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }
    }
}
