// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCourseAttachments()
        {
            // given
            IQueryable<CourseAttachment> randomCourseAttachments =
                CreateRandomCourseAttachments();

            IQueryable<CourseAttachment> storageCourseAttachments =
                randomCourseAttachments;

            IQueryable<CourseAttachment> expectedCourseAttachments =
                storageCourseAttachments;

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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
