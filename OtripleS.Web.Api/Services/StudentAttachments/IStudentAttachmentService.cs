//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Services.StudentAttachments
{
    public interface IStudentAttachmentService
    {
        ValueTask<StudentAttachment> AddStudentAttachmentAsync(StudentAttachment studentAttachment);
        IQueryable<StudentAttachment> RetrieveAllStudentAttachments();
        ValueTask<StudentAttachment> RetrieveStudentAttachmentByIdAsync
            (Guid studentId, Guid attachmentId);
    }
}
