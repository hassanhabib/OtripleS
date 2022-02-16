﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Services.Foundations.AssignmentAttachments;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAssignmentAttachmentService assignmentAttachmentService;

        public AssignmentAttachmentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.assignmentAttachmentService = new AssignmentAttachmentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static AssignmentAttachment CreateRandomAssignmentAttachment() =>
            CreateAssignmentAttachmentFiller(dates: GetRandomDateTime()).Create();

        private static IQueryable<AssignmentAttachment> CreateRandomAssignmentAttachments() =>
            CreateAssignmentAttachmentFiller(dates: GetRandomDateTime()).Create(GetRandomNumber()).AsQueryable();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 150).GetValue();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static AssignmentAttachment CreateRandomAssignmentAttachment(DateTimeOffset dates) =>
            CreateAssignmentAttachmentFiller(dates).Create();

        private static SqlException GetSqlException() =>
         (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomMessage() => new MnemonicString().GetValue();

        private static Filler<AssignmentAttachment> CreateAssignmentAttachmentFiller(DateTimeOffset dates)
        {
            var filler = new Filler<AssignmentAttachment>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates)
                .OnProperty(assignmentAttachment => assignmentAttachment.Assignment).IgnoreIt()
                .OnProperty(assignmentAttachment => assignmentAttachment.Attachment).IgnoreIt();

            return filler;
        }
    }
}
