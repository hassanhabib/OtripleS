// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Attachments;

namespace OtripleS.Web.Api.Services.Attachments
{
	public interface IAttachmentService
	{
		ValueTask<Attachment> AddAttachmentAsync(Attachment attachment);
		ValueTask<Attachment> RetrieveAttachmentByIdAsync(Guid attachmentId);
		IQueryable<Attachment> RetrieveAllAttachments();
		ValueTask<Attachment> ModifyAttachmentAsync(Attachment attachment);
		ValueTask<Attachment> RemoveAttachmentByIdAsync(Guid attachmentId);
	}
}
