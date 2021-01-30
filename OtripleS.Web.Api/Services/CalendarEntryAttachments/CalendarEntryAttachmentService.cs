// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService : ICalendarEntryAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public CalendarEntryAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<CalendarEntryAttachment> AddCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CalendarEntryAttachment> RetrieveCalendarEntryAttachmentByIdAsync
            (Guid calendarEntryId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryAttachmentIds(calendarEntryId, attachmentId);

            CalendarEntryAttachment storageCalendarEntryAttachment =
                await this.storageBroker.SelectCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

            ValidateStorageCalendarEntryAttachment(storageCalendarEntryAttachment, calendarEntryId, attachmentId);

            return storageCalendarEntryAttachment;
        });

        public ValueTask<CalendarEntryAttachment> RemoveCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryAttachmentIds(calendarEntryId, attachmentId);

            CalendarEntryAttachment maybeCalendarEntryAttachment =
                await this.storageBroker.SelectCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

            ValidateStorageCalendarEntryAttachment(maybeCalendarEntryAttachment, calendarEntryId, attachmentId);

            return await this.storageBroker.DeleteCalendarEntryAttachmentAsync(maybeCalendarEntryAttachment);
        });
    }
}
