// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Attachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.CalendarEntries;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.CalendarEntriesAttachments;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.Calendars;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CalendarEntryAttachments
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CalendarEntryAttachmentsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public CalendarEntryAttachmentsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<CalendarEntryAttachment> CreateRandomCalendarEntryAttachment()
        {
            CalendarEntry persistedCalendarEntry = await PostCalendarEntryAsync();
            Attachment persistedAttachment = await PostAttachmentAsync();

            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachmentFiller(
                persistedCalendarEntry.Id,
                persistedAttachment.Id).Create();

            return randomCalendarEntryAttachment;
        }

        private async Task<CalendarEntryAttachment> PostCalendarEntryAttachmentAsync()
        {
            CalendarEntryAttachment randomCalendarEntryAttachment = await CreateRandomCalendarEntryAttachment();
            await this.otripleSApiBroker.PostCalendarEntryAttachmentAsync(randomCalendarEntryAttachment);

            return randomCalendarEntryAttachment;
        }

        private async ValueTask<CalendarEntryAttachment> DeleteCalendarEntryAttachmentAsync(CalendarEntryAttachment calendarEntryAttachment)
        {
            CalendarEntryAttachment deletedCalendarEntryAttachment =
                await this.otripleSApiBroker.DeleteCalendarEntryAttachmentByIdsAsync(
                    calendarEntryAttachment.CalendarEntryId,
                    calendarEntryAttachment.AttachmentId);

            await this.otripleSApiBroker.DeleteAttachmentByIdAsync(calendarEntryAttachment.AttachmentId);
            await this.otripleSApiBroker.DeleteCalendarEntryByIdAsync(calendarEntryAttachment.CalendarEntryId);

            return deletedCalendarEntryAttachment;
        }

        private static Attachment CreateRandomAttachment() => CreateRandomAttachmentFiller().Create();

        private async ValueTask<CalendarEntry> PostCalendarEntryAsync()
        {
            CalendarEntry randomCalendarEntry = await CreateRandomCalendarEntryAsync();
            CalendarEntry inputCalendarEntry = randomCalendarEntry;

            return await this.otripleSApiBroker.PostCalendarEntryAsync(inputCalendarEntry);
        }

        private async ValueTask<Attachment> PostAttachmentAsync()
        {
            Attachment randomAttachment = CreateRandomAttachment();
            Attachment inputAttachment = randomAttachment;

            return await this.otripleSApiBroker.PostAttachmentAsync(inputAttachment);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 5).GetValue();


        private static Filler<CalendarEntryAttachment> CreateRandomCalendarEntryAttachmentFiller(
            Guid calendarEntryId, Guid attachmentId)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<CalendarEntryAttachment>();
            var note = GetRandomString();

            filler.Setup()
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.CalendarEntryId).Use(calendarEntryId)
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.AttachmentId).Use(attachmentId)
                .OnProperty(calendarEntryAttachment => calendarEntryAttachment.Notes).Use(note)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static string GetRandomString() => new MnemonicString().GetValue();

        private async ValueTask<CalendarEntry> PostRandomCalendarEntryAsync()
        {
            CalendarEntry randomCalendarEntry = await CreateRandomCalendarEntryAsync();
            await this.otripleSApiBroker.PostCalendarEntryAsync(randomCalendarEntry);

            return randomCalendarEntry;
        }


        private async ValueTask<CalendarEntry> CreateRandomCalendarEntryAsync()
        {
            Calendar calendar = await PostRandomCalendarAsync();
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<CalendarEntry>();

            filler.Setup()
                .OnProperty(calendarEntry => calendarEntry.Label).Use(GetRandomString())
                .OnProperty(calendarEntry => calendarEntry.CreatedBy).Use(calendar.CreatedBy)
                .OnProperty(calendarEntry => calendarEntry.UpdatedBy).Use(calendar.CreatedBy)
                .OnProperty(calendarEntry => calendarEntry.CreatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.UpdatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.CalendarId).Use(calendar.Id)
                .OnProperty(calenderEntry => calenderEntry.RepeatUntil).IgnoreIt()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }

        private async ValueTask<Calendar> PostRandomCalendarAsync()
        {
            Calendar randomCalendar = CreateRandomCalendar();
            await this.otripleSApiBroker.PostCalendarAsync(randomCalendar);

            return randomCalendar;
        }

        private static Filler<Attachment> CreateRandomAttachmentFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Attachment>();

            filler.Setup()
                .OnProperty(attachment => attachment.CreatedBy).Use(posterId)
                .OnProperty(attachment => attachment.UpdatedBy).Use(posterId)
                .OnProperty(attachment => attachment.CreatedDate).Use(now)
                .OnProperty(attachment => attachment.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Calendar CreateRandomCalendar()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid posterId = Guid.NewGuid();
            var filler = new Filler<Calendar>();

            filler.Setup()
                .OnProperty(calendarEntry => calendarEntry.CreatedBy).Use(posterId)
                .OnProperty(calendarEntry => calendarEntry.UpdatedBy).Use(posterId)
                .OnProperty(calendarEntry => calendarEntry.CreatedDate).Use(now)
                .OnProperty(calendarEntry => calendarEntry.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler.Create();
        }
    }
}