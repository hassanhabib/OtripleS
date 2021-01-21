//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.TeacherAttachments
{
    public partial class TeacherAttachmentService
    {
        private void ValidateTeacherAttachmentOnCreate(TeacherAttachment teacherAttachment)
        {
            ValidateTeacherAttachmentIsNull(teacherAttachment);
        }

        private void ValidateTeacherAttachmentIsNull(TeacherAttachment teacherContact)
        {
            if (teacherContact is null)
            {
                throw new NullTeacherAttachmentException();
            }
        }
    }
}
