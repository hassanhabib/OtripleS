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
        private void ValidateCourseAttachmentOnCreate(CourseAttachment courseAttachment)
        {
            ValidateCourseAttachmentIsNull(courseAttachment);

            ValidateCourseAttachmentIds(courseAttachment.CourseId);

        }

        private void ValidateCourseAttachmentIsNull(CourseAttachment courseAttachment)
        {
            if (courseAttachment is null)
            {
                throw new NullCourseAttachmentException();
            }
        }

        private void ValidateCourseAttachmentIds(Guid courseId)
        {
            if (courseId == default)
            {
                throw new InvalidCourseAttachmentException(
                    parameterName: nameof(CourseAttachment.CourseId),
                    parameterValue: courseId);
            }

        }
    }
}
