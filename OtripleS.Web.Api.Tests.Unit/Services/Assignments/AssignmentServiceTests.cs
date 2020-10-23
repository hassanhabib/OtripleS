// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Services.Assignments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Assignments
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
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Assignment CreateRandomAssignment(DateTimeOffset dates) =>
            CreateAssignmentFiller(dates).Create();

        private static Filler<Assignment> CreateAssignmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Assignment>();

            filler.Setup()
                .OnProperty(assignment => assignment.CreatedDate).Use(dates)
                .OnProperty(assignment => assignment.UpdatedDate).Use(dates)
                .OnProperty(assignment => assignment.Deadline).Use(dates.AddDays(GetRandomNumber()));

            return filler;
        }

        private static int GetRandomNumber() => new IntRange(min: 1, max: 150).GetValue();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                expectedException.Message == actualException.Message
                && expectedException.InnerException.Message == actualException.InnerException.Message;
        }

        private Assignment CreateRandomAssignment() =>
            CreateAssignmentFiller(dates: DateTimeOffset.UtcNow).Create();

        private static IQueryable<Assignment> CreateRandomAssignments(DateTimeOffset dates) =>
            CreateAssignmentFiller(dates).Create(GetRandomNumber()).AsQueryable();

        public static IEnumerable<object[]> InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new List<object[]>
            {
                new object[] { randomMoreThanMinuteFromNow },
                new object[] { randomMoreThanMinuteBeforeNow }
            };
        }

        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
    }
}
