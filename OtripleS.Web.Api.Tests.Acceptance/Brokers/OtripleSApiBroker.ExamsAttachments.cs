// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.ExamsAttachments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string ExamAttachmentsRelativeUrl = "api/examsattachments";

        public async ValueTask<ExamAttachment> PostExamAttachmentAsync(ExamAttachment examAttachment) =>
            await this.apiFactoryClient.PostContentAsync(ExamAttachmentsRelativeUrl, examAttachment);

        public async ValueTask<ExamAttachment> GetExamAttachmentByIdsAsync(Guid examId, Guid attachmentId) =>
            await this.apiFactoryClient.GetContentAsync<ExamAttachment>(
                $"{ExamAttachmentsRelativeUrl}/exams/{examId}/attachments/{attachmentId}");

        public async ValueTask<ExamAttachment> DeleteExamAttachmentByIdsAsync(Guid examId, Guid attachmentId) =>
            await this.apiFactoryClient.DeleteContentAsync<ExamAttachment>(
                $"{ExamAttachmentsRelativeUrl}/exams/{examId}/attachments/{attachmentId}");

        public async ValueTask<List<ExamAttachment>> GetAllExamAttachmentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<ExamAttachment>>($"{ExamAttachmentsRelativeUrl}/");
    }
}
