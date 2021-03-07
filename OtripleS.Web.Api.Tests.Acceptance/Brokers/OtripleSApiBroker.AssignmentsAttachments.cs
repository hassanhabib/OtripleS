// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.AssignmentsAttachments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string AssignmentAttachmentsRelativeUrl = "api/assignmentsattachments";

        public async ValueTask<AssignmentAttachment> PostAssignmentAttachmentAsync(
            AssignmentAttachment assignmentAttachment)
        {
            return await this.apiFactoryClient.PostContentAsync(
                AssignmentAttachmentsRelativeUrl,
                assignmentAttachment);
        }

        public async ValueTask<AssignmentAttachment> GetAssignmentAttachmentByIdsAsync(
            Guid assignmentId,
            Guid attachmentId)
        {
            return await this.apiFactoryClient.GetContentAsync<AssignmentAttachment>(
                $"{AssignmentAttachmentsRelativeUrl}/assignments/{assignmentId}/attachments/{attachmentId}");
        }

        public async ValueTask<AssignmentAttachment> DeleteAssignmentAttachmentByIdsAsync(
            Guid assignmentId,
            Guid attachmentId)
        {
            return await this.apiFactoryClient.DeleteContentAsync<AssignmentAttachment>(
                $"{AssignmentAttachmentsRelativeUrl}/assignments/{assignmentId}/attachments/{attachmentId}");
        }

        public async ValueTask<List<AssignmentAttachment>> GetAllAssignmentAttachmentsAsync()
        {
            return await this.apiFactoryClient.GetContentAsync<List<AssignmentAttachment>>(
                $"{AssignmentAttachmentsRelativeUrl}/");
        }
    }
}
