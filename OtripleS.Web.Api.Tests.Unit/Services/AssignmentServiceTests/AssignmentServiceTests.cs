// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Services.Assignments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentServiceTests
{
    public partial class AssignmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IAssignmentService assignmentService;

        public AssignmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.assignmentService = new AssignmentService(
                storageBroker: storageBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object
            );
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
        private static IQueryable<Assignment> CreateRandomAssignments(DateTimeOffset dates) =>
            CreateAssignmentFiller(dates).Create(GetRandomNumber()).AsQueryable();
        private static Filler<Assignment> CreateAssignmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Assignment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();
    }
}