//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.ExamAttachments
{
    public partial class ExamAttachmentService
    {
        public void ValidateExamAttachmentOnCreate(ExamAttachment examAttachment)
        {
            ValidateExamAttachmentIsNull(examAttachment);
        }

        private void ValidateExamAttachmentIsNull(ExamAttachment examAttachment)
        {
            if (examAttachment is null)
            {
                throw new NullExamAttachmentException();
            }
        }
    }
}
