﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllCalendarEntryAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedCalendarEntryAttachmentDependencyException =
                new CalendarEntryAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntryAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllCalenderEntryAttachmentAction = () =>
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments();

            // then
            Assert.Throws<CalendarEntryAttachmentDependencyException>(
                retrieveAllCalenderEntryAttachmentAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntryAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllCalendarEntryAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedCalendarEntryAttachmentService =
                new FailedCalendarEntryAttachmentServiceException(serviceException);

            var expectedCalendarEntryAttachmentServiceException =
                new CalendarEntryAttachmentServiceException(failedCalendarEntryAttachmentService);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntryAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllCalenderEntryAttachmentAction = () =>
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments();

            // then
            Assert.Throws<CalendarEntryAttachmentServiceException>(
                retrieveAllCalenderEntryAttachmentAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntryAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
