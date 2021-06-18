// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Foundations.ExamsAttachments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.ExamsAttachments
{
    public partial class ExamAttachmentsApiTests
    {
        [Fact]
        public async Task ShouldPostExamAttachmentAsync()
        {
            // given
            ExamAttachment randomExamAttachment = await CreateRandomExamAttachment();
            ExamAttachment inputExamAttachment = randomExamAttachment;
            ExamAttachment expectedExamAttachment = inputExamAttachment;

            // when             
            ExamAttachment actualExamAttachment =
                await this.otripleSApiBroker.PostExamAttachmentAsync(inputExamAttachment);

            ExamAttachment retrievedExamAttachment =
                await this.otripleSApiBroker.GetExamAttachmentByIdsAsync(
                    inputExamAttachment.ExamId,
                    inputExamAttachment.AttachmentId);

            // then
            actualExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);
            retrievedExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);
            await DeleteExamAttachmentAsync(actualExamAttachment);
        }

        [Fact]
        public async Task ShouldGetAllExamAttachmentsAsync()
        {
            // given
            var randomExamAttachments = new List<ExamAttachment>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                ExamAttachment randomExamAttachment = await PostExamAttachmentAsync();
                randomExamAttachments.Add(randomExamAttachment);
            }

            List<ExamAttachment> inputExamAttachments = randomExamAttachments;
            List<ExamAttachment> expectedExamAttachments = inputExamAttachments;

            // when
            List<ExamAttachment> actualExamAttachments =
                await this.otripleSApiBroker.GetAllExamAttachmentsAsync();

            // then
            foreach (ExamAttachment expectedExamAttachment in expectedExamAttachments)
            {
                ExamAttachment actualExamAttachment =
                    actualExamAttachments.Single(studentAttachment =>
                        studentAttachment.ExamId == expectedExamAttachment.ExamId);

                actualExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);

                await DeleteExamAttachmentAsync(actualExamAttachment);
            }
        }

        [Fact]
        public async Task ShouldDeleteExamAttachmentAsync()
        {
            // given
            ExamAttachment randomExamAttachment = await PostExamAttachmentAsync();
            ExamAttachment inputExamAttachment = randomExamAttachment;
            ExamAttachment expectedExamAttachment = inputExamAttachment;

            // when 
            ExamAttachment deletedExamAttachment =
                await DeleteExamAttachmentAsync(inputExamAttachment);

            ValueTask<ExamAttachment> getExamAttachmentByIdTask =
                this.otripleSApiBroker.GetExamAttachmentByIdsAsync(
                    inputExamAttachment.ExamId,
                    inputExamAttachment.AttachmentId);

            // then
            deletedExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getExamAttachmentByIdTask.AsTask());
        }
    }
}
