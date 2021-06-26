// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.CoursesAttachments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CoursesAttachments
{
    public partial class CourseAttachmentsApiTests
    {
        [Fact]
        public async Task ShouldPostCourseAttachmentAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = await CreateRandomCourseAttachment();
            CourseAttachment inputCourseAttachment = randomCourseAttachment;
            CourseAttachment expectedCourseAttachment = inputCourseAttachment;

            // when             
            CourseAttachment actualCourseAttachment =
                await this.otripleSApiBroker.PostCourseAttachmentAsync(inputCourseAttachment);

            CourseAttachment retrievedCourseAttachment =
                await this.otripleSApiBroker.GetCourseAttachmentByIdsAsync(
                    inputCourseAttachment.CourseId,
                    inputCourseAttachment.AttachmentId);

            // then
            actualCourseAttachment.Should().BeEquivalentTo(expectedCourseAttachment);
            retrievedCourseAttachment.Should().BeEquivalentTo(expectedCourseAttachment);
            await DeleteCourseAttachmentAsync(actualCourseAttachment);
        }

        [Fact]
        public async Task ShouldGetAllCourseAttachmentsAsync()
        {
            // given
            var randomCourseAttachments = new List<CourseAttachment>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                CourseAttachment randomCourseAttachment = await PostCourseAttachmentAsync();
                randomCourseAttachments.Add(randomCourseAttachment);
            }

            List<CourseAttachment> inputCourseAttachments = randomCourseAttachments;
            List<CourseAttachment> expectedCourseAttachments = inputCourseAttachments;

            // when
            List<CourseAttachment> actualCourseAttachments =
                await this.otripleSApiBroker.GetAllCourseAttachmentsAsync();

            // then
            foreach (CourseAttachment expectedCourseAttachment in expectedCourseAttachments)
            {
                CourseAttachment actualCourseAttachment =
                    actualCourseAttachments.Single(studentAttachment =>
                        studentAttachment.CourseId == expectedCourseAttachment.CourseId);

                actualCourseAttachment.Should().BeEquivalentTo(expectedCourseAttachment);

                await DeleteCourseAttachmentAsync(actualCourseAttachment);
            }
        }

        [Fact]
        public async Task ShouldDeleteCourseAttachmentAsync()
        {
            // given
            CourseAttachment randomCourseAttachment = await PostCourseAttachmentAsync();
            CourseAttachment inputCourseAttachment = randomCourseAttachment;
            CourseAttachment expectedCourseAttachment = inputCourseAttachment;

            // when 
            CourseAttachment deletedCourseAttachment =
                await DeleteCourseAttachmentAsync(inputCourseAttachment);

            ValueTask<CourseAttachment> getCourseAttachmentByIdTask =
                this.otripleSApiBroker.GetCourseAttachmentByIdsAsync(
                    inputCourseAttachment.CourseId,
                    inputCourseAttachment.AttachmentId);

            // then
            deletedCourseAttachment.Should().BeEquivalentTo(expectedCourseAttachment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCourseAttachmentByIdTask.AsTask());
        }
    }
}
