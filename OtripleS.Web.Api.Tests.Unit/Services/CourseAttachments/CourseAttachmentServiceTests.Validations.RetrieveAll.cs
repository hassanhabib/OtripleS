using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenCourseAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<CourseAttachment> emptyStorageCourseAttachments =
                new List<CourseAttachment>().AsQueryable();

            IQueryable<CourseAttachment> storageCourseAttachments =
                emptyStorageCourseAttachments;

            IQueryable<CourseAttachment> expectedCourseAttachments =
                emptyStorageCourseAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourseAttachments())
                    .Returns(storageCourseAttachments);

            // when
            IQueryable<CourseAttachment> actualCourseAttachments =
                this.courseAttachmentService.RetrieveAllCourseAttachments();

            // then
            actualCourseAttachments.Should().BeEquivalentTo(expectedCourseAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourseAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No course attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
