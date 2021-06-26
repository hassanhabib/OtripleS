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
        public ValueTask<StudentAttachment> InsertStudentAttachmentAsync(
           StudentAttachment studentAttachment);

        public IQueryable<StudentAttachment> SelectAllStudentAttachments();

        public ValueTask<StudentAttachment> SelectStudentAttachmentByIdAsync(
            Guid studentId,
            Guid attachmentId);

        public ValueTask<StudentAttachment> UpdateStudentAttachmentAsync(
            StudentAttachment studentAttachment);

        public ValueTask<StudentAttachment> DeleteStudentAttachmentAsync(
            StudentAttachment studentAttachment);
    }
}
