//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Services.Foundations.TeacherAttachments
{
    public interface ITeacherAttachmentService
    {
        ValueTask<TeacherAttachment> AddTeacherAttachmentAsync(TeacherAttachment teacherAttachment);
        IQueryable<TeacherAttachment> RetrieveAllTeacherAttachments();
        ValueTask<TeacherAttachment> RetrieveTeacherAttachmentByIdAsync(Guid teacherId, Guid attachmentId);
        ValueTask<TeacherAttachment> RemoveTeacherAttachmentByIdAsync(Guid teacherId, Guid attachmentId);
    }
}
