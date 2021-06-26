// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string TeacherAttachmentsRelativeUrl = "api/teacherattachments";

        public async ValueTask<TeacherAttachment> PostTeacherAttachmentAsync(TeacherAttachment teacherAttachment) =>
            await this.apiFactoryClient.PostContentAsync(TeacherAttachmentsRelativeUrl, teacherAttachment);

        public async ValueTask<TeacherAttachment> GetTeacherAttachmentByIdsAsync(Guid teacherId, Guid attachmentId) =>
            await this.apiFactoryClient.GetContentAsync<TeacherAttachment>(
                $"{TeacherAttachmentsRelativeUrl}/teachers/{teacherId}/attachments/{attachmentId}");

        public async ValueTask<TeacherAttachment> DeleteTeacherAttachmentByIdsAsync(Guid teacherId, Guid attachmentId) =>
            await this.apiFactoryClient.DeleteContentAsync<TeacherAttachment>(
                $"{TeacherAttachmentsRelativeUrl}/teachers/{teacherId}/attachments/{attachmentId}");

        public async ValueTask<List<TeacherAttachment>> GetAllTeacherAttachmentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<TeacherAttachment>>($"{TeacherAttachmentsRelativeUrl}/");
    }
}
