// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.TeacherAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<TeacherAttachment> InsertTeacherAttachmentAsync(
          TeacherAttachment teacherAttachment);

        IQueryable<TeacherAttachment> SelectAllTeacherAttachments();

        ValueTask<TeacherAttachment> SelectTeacherAttachmentByIdAsync(
           Guid teacherId,
           Guid attachmentId);

        ValueTask<TeacherAttachment> UpdateTeacherAttachmentAsync(
           TeacherAttachment teacherAttachment);

        ValueTask<TeacherAttachment> DeleteTeacherAttachmentAsync(
           TeacherAttachment teacherAttachment);
    }
}