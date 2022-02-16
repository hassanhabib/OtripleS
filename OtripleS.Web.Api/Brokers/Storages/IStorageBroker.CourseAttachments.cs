// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<CourseAttachment> InsertCourseAttachmentAsync(
           CourseAttachment courseAttachment);

        IQueryable<CourseAttachment> SelectAllCourseAttachments();

        ValueTask<CourseAttachment> SelectCourseAttachmentByIdAsync(
           Guid courseId,
           Guid attachmentId);

        ValueTask<CourseAttachment> UpdateCourseAttachmentAsync(
            CourseAttachment courseAttachment);

        ValueTask<CourseAttachment> DeleteCourseAttachmentAsync(
            CourseAttachment courseAttachment);
    }
}