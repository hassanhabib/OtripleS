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
            ValidateTeacherAttachmentIdIsNull(teacherAttachment.TeacherId, teacherAttachment.AttachmentId);
        }

        private void ValidateTeacherAttachmentIsNull(TeacherAttachment teacherContact)
        {
            if (teacherContact is null)
            {
                throw new NullTeacherAttachmentException();
            }
        }

        private void ValidateTeacherAttachmentIdIsNull(Guid teacherId, Guid attachmentId)
        {
            if (teacherId == default)
            {
                throw new InvalidTeacherAttachmentException(
                    parameterName: nameof(TeacherAttachment.TeacherId),
                    parameterValue: teacherId);
            }
            else if (attachmentId == default)
            {
                throw new InvalidTeacherAttachmentException(
                    parameterName: nameof(TeacherAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageTeacherAttachment(
            TeacherAttachment storageTeacherAttachment,
            Guid studentId, 
            Guid attachmentId)
        {
            if (storageTeacherAttachment == null)
            {
                throw new NotFoundTeacherAttachmentException(studentId, attachmentId);
            }
        }
    }
}
