// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;

namespace OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService : ICalendarEntryAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        public CalendarEntryAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<CalendarEntryAttachment> AddCalendarEntryAttachmentAsync(
            CalendarEntryAttachment calendarEntryAttachment) => TryCatch(async () =>
        {
            ValidateCalendarEntryAttachmentOnAdd(calendarEntryAttachment);

            return await this.storageBroker.InsertCalendarEntryAttachmentAsync(calendarEntryAttachment);
        });
        public IQueryable<CalendarEntryAttachment> RetrieveAllCalendarEntryAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllCalendarEntryAttachments());
        public ValueTask<CalendarEntryAttachment> RetrieveCalendarEntryAttachmentByIdAsync
            (Guid calendarEntryId, Guid attachmentId) => TryCatch(async () =>
        {
            ValidateCalendarEntryAttachmentIds(calendarEntryId, attachmentId);

            CalendarEntryAttachment maybeCalendarEntryAttachment =
                await this.storageBroker.SelectCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

            ValidateStorageCalendarEntryAttachment(maybeCalendarEntryAttachment, calendarEntryId, attachmentId);

            return maybeCalendarEntryAttachment;
        });
        public ValueTask<CalendarEntryAttachment> RemoveCalendarEntryAttachmentByIdAsync(
            Guid calendarEntryId, Guid attachmentId) => TryCatch(async () =>
        {
            ValidateCalendarEntryAttachmentIds(calendarEntryId, attachmentId);

            CalendarEntryAttachment maybeCalendarEntryAttachment =
                await this.storageBroker.SelectCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

            ValidateStorageCalendarEntryAttachment(maybeCalendarEntryAttachment, calendarEntryId, attachmentId);

            return await this.storageBroker.DeleteCalendarEntryAttachmentAsync(maybeCalendarEntryAttachment);
        });
    }
}
