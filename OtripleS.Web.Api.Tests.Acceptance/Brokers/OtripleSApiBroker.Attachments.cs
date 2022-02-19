// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attachments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string AttachmentsRelativeUrl = "api/attachments";

        public async ValueTask<Attachment> PostAttachmentAsync(Attachment attachment) =>
            await this.apiFactoryClient.PostContentAsync(AttachmentsRelativeUrl, attachment);

        public async ValueTask<Attachment> GetAttachmentByIdAsync(Guid attachmentId) =>
            await this.apiFactoryClient.GetContentAsync<Attachment>($"{AttachmentsRelativeUrl}/{attachmentId}");

        public async ValueTask<Attachment> DeleteAttachmentByIdAsync(Guid attachmentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Attachment>($"{AttachmentsRelativeUrl}/{attachmentId}");

        public async ValueTask<Attachment> PutAttachmentAsync(Attachment attachment) =>
            await this.apiFactoryClient.PutContentAsync(AttachmentsRelativeUrl, attachment);

        public async ValueTask<List<Attachment>> GetAllAttachmentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Attachment>>($"{AttachmentsRelativeUrl}/");
    }
}