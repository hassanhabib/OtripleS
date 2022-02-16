// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Services.Foundations.Assignments;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Assignments
{
    public partial class AssignmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAssignmentService assignmentService;

        public AssignmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.assignmentService = new AssignmentService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Assignment CreateRandomAssignment(DateTimeOffset dates) =>
            CreateAssignmentFiller(dates).Create();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Expression<Func<Xeption, bool>> SameValidationExceptionAs(Xeption expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Assignment CreateRandomAssignment() =>
            CreateAssignmentFiller(dates: GetRandomDateTime()).Create();

        private static IQueryable<Assignment> CreateRandomAssignments(DateTimeOffset dates) =>
            CreateAssignmentFiller(dates).Create(GetRandomNumber()).AsQueryable();

        public static TheoryData InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new TheoryData<int>
            {
                randomMoreThanMinuteFromNow,
                randomMoreThanMinuteBeforeNow
            };
        }

        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();
        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Filler<Assignment> CreateAssignmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Assignment>();

            filler.Setup()
                .OnProperty(assignment => assignment.Status).Use(AssignmentStatus.Active)
                .OnProperty(assignment => assignment.CreatedDate).Use(dates)
                .OnProperty(assignment => assignment.UpdatedDate).Use(dates)
                .OnProperty(assignment => assignment.Deadline).Use(dates.AddDays(GetRandomNumber()))
                .OnProperty(assignment => assignment.AssignmentAttachments).IgnoreIt();

            return filler;
        }
    }
}
