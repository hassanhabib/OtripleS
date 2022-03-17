// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Attachments;

namespace OtripleS.Web.Api.Services.Foundations.Attachments
{
    public interface IAttachmentService
    {
        ValueTask<Attachment> AddAttachmentAsync(Attachment attachment);
        IQueryable<Attachment> RetrieveAllAttachments();
        ValueTask<Attachment> RetrieveAttachmentByIdAsync(Guid attachmentId);
        ValueTask<Attachment> ModifyAttachmentAsync(Attachment attachment);
        ValueTask<Attachment> RemoveAttachmentByIdAsync(Guid attachmentId);
    }
}
