//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenCalendarEntryAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<CalendarEntryAttachment> emptyStorageCalendarEntryAttachments =
                new List<CalendarEntryAttachment>().AsQueryable();

            IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachments =
                emptyStorageCalendarEntryAttachments;

            IQueryable<CalendarEntryAttachment> expectedCalendarEntryAttachments =
                emptyStorageCalendarEntryAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCalendarEntryAttachments())
                    .Returns(storageCalendarEntryAttachments);

            // when
            IQueryable<CalendarEntryAttachment> actualCalendarEntryAttachments =
                this.calendarEntryAttachmentService.RetrieveAllCalendarEntryAttachments();

            // then
            actualCalendarEntryAttachments.Should().BeEquivalentTo(expectedCalendarEntryAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCalendarEntryAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No calendarentry attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
