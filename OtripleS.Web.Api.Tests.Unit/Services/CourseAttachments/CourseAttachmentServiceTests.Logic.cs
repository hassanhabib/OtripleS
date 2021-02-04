// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCourseAttachmentById()
        {
            // given
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            CourseAttachment storageCourseAttachment = randomCourseAttachment;
            CourseAttachment expectedCourseAttachment = storageCourseAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseAttachmentByIdAsync(
                    randomCourseAttachment.CourseId, randomCourseAttachment.AttachmentId))
                        .Returns(new ValueTask<CourseAttachment>(randomCourseAttachment));

            // when
            CourseAttachment actualCourseAttachment = await
                this.courseAttachmentService.RetrieveCourseAttachmentByIdAsync(
                    randomCourseAttachment.CourseId, randomCourseAttachment.AttachmentId);

            // then
            actualCourseAttachment.Should().BeEquivalentTo(expectedCourseAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(
                    randomCourseAttachment.CourseId, randomCourseAttachment.AttachmentId),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
