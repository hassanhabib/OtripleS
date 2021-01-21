//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Services.TeacherAttachments
{
    public interface ITeacherAttachmentService
    {
        ValueTask<TeacherAttachment> AddTeacherAttachmentAsync(TeacherAttachment teacherAttachment);
        ValueTask<TeacherAttachment> RetrieveTeacherAttachmentByIdAsync(Guid teacherId, Guid attachmentId);
    }
}
