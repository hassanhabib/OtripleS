//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Foundations.StudentAttachments;
using OtripleS.Web.Api.Models.Foundations.StudentAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentService
    {

        private static void ValidateStudentAttachmentOnCreate(StudentAttachment studentAttachment)
        {
            ValidateStudentAttachmentIsNull(studentAttachment);
            ValidateStudentAttachmentIdIsNull(studentAttachment.StudentId, studentAttachment.AttachmentId);
        }

        private static void ValidateStudentAttachmentIsNull(StudentAttachment studentContact)
        {
            if (studentContact is null)
            {
                throw new NullStudentAttachmentException();
            }
        }

        private static void ValidateStudentAttachmentIdIsNull(Guid studentId, Guid attachmentId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentAttachmentException(
                    parameterName: nameof(StudentAttachment.StudentId),
                    parameterValue: studentId);
            }

            if (attachmentId == default)
            {
                throw new InvalidStudentAttachmentException(
                    parameterName: nameof(StudentAttachment.AttachmentId),
                    parameterValue: attachmentId);
            }
        }

        private static void ValidateStorageStudentAttachment(
            StudentAttachment storageStudentAttachment,
            Guid studentId, Guid attachmentId)
        {
            if (storageStudentAttachment == null)
            {
                throw new NotFoundStudentAttachmentException(studentId, attachmentId);
            }
        }

        private void ValidateStorageStudentAttachments(IQueryable<StudentAttachment> storageStudentAttachments)
        {
            if (!storageStudentAttachments.Any())
            {
                this.loggingBroker.LogWarning("No student attachments found in storage.");
            }
        }
    }
}
