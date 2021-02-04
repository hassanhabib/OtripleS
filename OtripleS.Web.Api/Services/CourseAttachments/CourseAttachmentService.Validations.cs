using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private void ValidateCourseAttachmentIds(Guid calendarEntryId, Guid attachmentId)
        {
            if (calendarEntryId == default)
            {
                throw new InvalidCourseAttachmentException(
                    parameterName: nameof(CourseAttachment.CourseId),
                    parameterValue: calendarEntryId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidCourseAttachmentException(
                    parameterName: nameof(CourseAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }
    }
}
