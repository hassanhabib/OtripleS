//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Foundations.CalendarEntryAttachments.Exceptions;
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

            // when . then
            Assert.Throws<CalendarEntryAttachmentDependencyException>(() =>
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntryAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllCalendarEntryAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedCalendarEntryAttachmentServiceException =
                new CalendarEntryAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntryAttachments())
                    .Throws(exception);

            // when . then
            Assert.Throws<CalendarEntryAttachmentServiceException>(() =>
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntryAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
