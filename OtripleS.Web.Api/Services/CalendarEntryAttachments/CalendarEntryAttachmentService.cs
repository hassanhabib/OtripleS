﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;

namespace OtripleS.Web.Api.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentService : ICalendarEntryAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public CalendarEntryAttachmentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<CalendarEntryAttachment> RemoveCalendarEntryAttachmentByIdAsync(Guid calendarEntryId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateCalendarEntryAttachmentIdIsNull(calendarEntryId, attachmentId);

            CalendarEntryAttachment mayBeCalendarAttachment =
                await this.storageBroker.SelectCalendarEntryAttachmentByIdAsync(calendarEntryId, attachmentId);

            ValidateStorageCalendarEntryAttachment(mayBeCalendarAttachment, calendarEntryId, attachmentId);

            return await this.storageBroker.DeleteCalendarEntryAttachmentAsync(mayBeCalendarAttachment);
        });
    }
}
