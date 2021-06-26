// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentAttachments;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentAttachment> InsertStudentAttachmentAsync(
          StudentAttachment studentAttachment);

        IQueryable<StudentAttachment> SelectAllStudentAttachments();

        ValueTask<StudentAttachment> SelectStudentAttachmentByIdAsync(
           Guid studentId,
           Guid attachmentId);

        ValueTask<StudentAttachment> UpdateStudentAttachmentAsync(
           StudentAttachment studentAttachment);

        ValueTask<StudentAttachment> DeleteStudentAttachmentAsync(
           StudentAttachment studentAttachment);
    }
}