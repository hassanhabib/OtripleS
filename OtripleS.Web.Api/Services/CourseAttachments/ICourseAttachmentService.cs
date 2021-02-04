//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public interface ICourseAttachmentService
    {
        ValueTask<CourseAttachment> AddCourseAttachmentAsync(CourseAttachment courseAttachment);
        ValueTask<CourseAttachment> RetrieveCourseAttachmentByIdAsync(Guid courseId, Guid attachmentId);
    }
}
