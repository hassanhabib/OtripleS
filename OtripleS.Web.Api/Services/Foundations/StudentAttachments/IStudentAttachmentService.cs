//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.StudentAttachments
{
    public interface IStudentAttachmentService
    {
        ValueTask<StudentAttachment> AddStudentAttachmentAsync(StudentAttachment studentAttachment);
        IQueryable<StudentAttachment> RetrieveAllStudentAttachments();

        ValueTask<StudentAttachment> RetrieveStudentAttachmentByIdAsync
            (Guid studentId, Guid attachmentId);

        ValueTask<StudentAttachment> RemoveStudentAttachmentByIdAsync(Guid studentId, Guid attachmentId);
    }
}
