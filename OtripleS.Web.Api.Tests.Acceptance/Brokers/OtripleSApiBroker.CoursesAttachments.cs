// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.CoursesAttachments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string CourseAttachmentsRelativeUrl = "api/coursesattachments";

        public async ValueTask<CourseAttachment> PostCourseAttachmentAsync(CourseAttachment courseAttachment) =>
            await this.apiFactoryClient.PostContentAsync(CourseAttachmentsRelativeUrl, courseAttachment);

        public async ValueTask<CourseAttachment> GetCourseAttachmentByIdsAsync(Guid courseId, Guid attachmentId) =>
            await this.apiFactoryClient.GetContentAsync<CourseAttachment>(
                $"{CourseAttachmentsRelativeUrl}/courses/{courseId}/attachments/{attachmentId}");

        public async ValueTask<CourseAttachment> DeleteCourseAttachmentByIdsAsync(Guid courseId, Guid attachmentId) =>
            await this.apiFactoryClient.DeleteContentAsync<CourseAttachment>(
                $"{CourseAttachmentsRelativeUrl}/courses/{courseId}/attachments/{attachmentId}");

        public async ValueTask<List<CourseAttachment>> GetAllCourseAttachmentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<CourseAttachment>>($"{CourseAttachmentsRelativeUrl}/");
    }
}
