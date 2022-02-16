﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Classrooms
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ClassroomsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public ClassroomsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static Classroom CreateRandomClassroom() =>
            CreateRandomClassroomFiller().Create();

        private async ValueTask<Classroom> PostRandomClassroomAsync()
        {
            Classroom randomClassroom = CreateRandomClassroom();
            await this.otripleSApiBroker.PostClassroomAsync(randomClassroom);

            return randomClassroom;
        }

        private static Classroom UpdateClassroomRandom(Classroom classroom)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.Id).Use(classroom.Id)
                .OnProperty(classroom => classroom.Status).Use(ClassroomStatus.Available)
                .OnProperty(classroom => classroom.CreatedBy).Use(classroom.CreatedBy)
                .OnProperty(classroom => classroom.UpdatedBy).Use(classroom.UpdatedBy)
                .OnProperty(classroom => classroom.CreatedDate).Use(classroom.CreatedDate)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Classroom> CreateRandomClassroomFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();

            var filler = new Filler<Classroom>();

            filler.Setup()
                .OnProperty(classroom => classroom.Status).Use(ClassroomStatus.Available)
                .OnProperty(classroom => classroom.CreatedBy).Use(posterId)
                .OnProperty(classroom => classroom.UpdatedBy).Use(posterId)
                .OnProperty(classroom => classroom.CreatedDate).Use(now)
                .OnProperty(classroom => classroom.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
