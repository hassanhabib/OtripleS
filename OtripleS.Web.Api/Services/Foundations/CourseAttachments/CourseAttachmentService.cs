// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentService : ICourseAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CourseAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<CourseAttachment> AddCourseAttachmentAsync(CourseAttachment courseAttachment) =>
        TryCatch(async () =>
        {
            ValidateCourseAttachmentOnAdd(courseAttachment);

            return await storageBroker.InsertCourseAttachmentAsync(courseAttachment);
        });

        public IQueryable<CourseAttachment> RetrieveAllCourseAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllCourseAttachments());

        public ValueTask<CourseAttachment> RetrieveCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateCourseAttachmentIds(courseId, attachmentId);

            CourseAttachment maybeCourseAttachment =
                await this.storageBroker.SelectCourseAttachmentByIdAsync(courseId, attachmentId);

            ValidateStorageCourseAttachment(maybeCourseAttachment, courseId, attachmentId);

            return maybeCourseAttachment;
        });

        public ValueTask<CourseAttachment> RemoveCourseAttachmentByIdAsync(
            Guid courseId,
            Guid attachmentId) => TryCatch(async () =>
        {
            ValidateCourseAttachmentIds(courseId, attachmentId);

            CourseAttachment maybeCourseAttachment =
                await this.storageBroker.SelectCourseAttachmentByIdAsync(courseId, attachmentId);

            ValidateStorageCourseAttachment(maybeCourseAttachment, courseId, attachmentId);

            return await this.storageBroker.DeleteCourseAttachmentAsync(maybeCourseAttachment);
        });

    }
}
