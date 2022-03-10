// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCalendarEntryAttachments()
        {
            // given
            IQueryable<CalendarEntryAttachment> randomCalendarEntryAttachments =
                CreateRandomCalendarEntryAttachments();

            IQueryable<CalendarEntryAttachment> storageCalendarEntryAttachments =
                randomCalendarEntryAttachments;

            IQueryable<CalendarEntryAttachment> expectedCalendarEntryAttachments =
                storageCalendarEntryAttachments;

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
