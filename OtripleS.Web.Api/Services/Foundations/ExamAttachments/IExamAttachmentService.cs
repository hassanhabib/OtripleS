//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.ExamAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.ExamAttachments
{
    public interface IExamAttachmentService
    {
        ValueTask<ExamAttachment> RemoveExamAttachmentByIdAsync(
          Guid examId,
          Guid attachmentId);
        ValueTask<ExamAttachment> AddExamAttachmentAsync(ExamAttachment someExamAttachment);
        IQueryable<ExamAttachment> RetrieveAllExamAttachments();

        ValueTask<ExamAttachment> RetrieveExamAttachmentByIdAsync
            (Guid examId, Guid attachmentId);
    }
}
