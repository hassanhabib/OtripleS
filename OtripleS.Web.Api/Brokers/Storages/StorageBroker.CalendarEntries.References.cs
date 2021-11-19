// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.CalendarEntries;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private static void SetCalendarEntryReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEntry>()
                .HasOne(entry => entry.Calendar)
                .WithMany(calendar => calendar.CalendarEntries)
                .HasForeignKey(entry => entry.CalendarId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
