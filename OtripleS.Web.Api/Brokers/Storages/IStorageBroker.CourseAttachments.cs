// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Foundations.CourseAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<CourseAttachment> InsertCourseAttachmentAsync(
            CourseAttachment calendarEntryAttachment);

        public IQueryable<CourseAttachment> SelectAllCourseAttachments();

        public ValueTask<CourseAttachment> SelectCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId);

        public ValueTask<CourseAttachment> UpdateCourseAttachmentAsync(
            CourseAttachment calendarEntryAttachment);

        public ValueTask<CourseAttachment> DeleteCourseAttachmentAsync(
            CourseAttachment calendarEntryAttachment);
    }
}
