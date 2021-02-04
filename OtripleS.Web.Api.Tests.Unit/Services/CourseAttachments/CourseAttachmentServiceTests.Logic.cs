//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveCourseAttachmentAsync()
        {
            // given
            var randomCourseId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            Guid inputAttachmentId = randomAttachmentId;
            CourseAttachment randomCourseAttachment = CreateRandomCourseAttachment();
            randomCourseAttachment.CourseId = inputCourseId;
            randomCourseAttachment.AttachmentId = inputAttachmentId;
            CourseAttachment storageCourseAttachment = randomCourseAttachment;
            CourseAttachment expectedCourseAttachment = storageCourseAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseAttachmentByIdAsync(inputCourseId, inputAttachmentId))
                    .ReturnsAsync(storageCourseAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCourseAttachmentAsync(storageCourseAttachment))
                    .ReturnsAsync(expectedCourseAttachment);

            // when
            CourseAttachment actualCourseAttachment =
                await this.courseAttachmentService.RemoveCourseAttachmentByIdAsync(
                    inputCourseId, inputAttachmentId);

            // then
            actualCourseAttachment.Should().BeEquivalentTo(expectedCourseAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseAttachmentByIdAsync(inputCourseId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAttachmentAsync(storageCourseAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
