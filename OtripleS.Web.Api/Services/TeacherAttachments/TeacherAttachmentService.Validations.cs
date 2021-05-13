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
        private static void ValidateTeacherAttachmentOnCreate(TeacherAttachment teacherAttachment)
        {
            ValidateTeacherAttachmentIsNull(teacherAttachment);
            ValidateTeacherAttachmentIds(teacherAttachment.TeacherId, teacherAttachment.AttachmentId);
        }

        private static void ValidateTeacherAttachmentIsNull(TeacherAttachment teacherContact)
        {
            if (teacherContact is null)
            {
                throw new NullTeacherAttachmentException();
            }
        }

        private static void ValidateTeacherAttachmentIds(Guid teacherId, Guid attachmentId)
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
            if (storageTeacherAttachment is null)
            {
                throw new NotFoundTeacherAttachmentException(studentId, attachmentId);
            }
        }

        private void ValidateStorageTeacherAttachments(IQueryable<TeacherAttachment> storageTeacherAttachments)
        {
            if (!storageTeacherAttachments.Any())
            {
                this.loggingBroker.LogWarning("No teacher attachments found in storage.");
            }
        }
    }
}
