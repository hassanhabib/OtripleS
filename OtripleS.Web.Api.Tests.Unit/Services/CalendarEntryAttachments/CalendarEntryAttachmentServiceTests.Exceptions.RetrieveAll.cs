// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
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

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(
                        expectedCalendarEntryAttachmentDependencyException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAllCalendarEntryAttachments(),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
