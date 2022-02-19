// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.ExamAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ExamAttachment> InsertExamAttachmentAsync(
           ExamAttachment examAttachment);

        IQueryable<ExamAttachment> SelectAllExamAttachments();

        ValueTask<ExamAttachment> SelectExamAttachmentByIdAsync(
            Guid examId,
            Guid attachmentId);

        ValueTask<ExamAttachment> UpdateExamAttachmentAsync(
            ExamAttachment examAttachment);

        ValueTask<ExamAttachment> DeleteExamAttachmentAsync(
            ExamAttachment examAttachment);
    }
}