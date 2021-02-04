using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public interface ICourseAttachmentService
    {
        ValueTask<CourseAttachment> RemoveCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId);
    }
}
