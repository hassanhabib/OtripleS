// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Attachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Attachment> InsertAttachmentAsync(Attachment attachment);
        IQueryable<Attachment> SelectAllAttachments();
        ValueTask<Attachment> SelectAttachmentByIdAsync(Guid attachmentId);
        ValueTask<Attachment> UpdateAttachmentAsync(Attachment attachment);
        ValueTask<Attachment> DeleteAttachmentAsync(Attachment attachment);
    }
}
