// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveExamAttachmentByIdAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment storageExamAttachment = randomExamAttachment;
            ExamAttachment expectedExamAttachment = storageExamAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamAttachmentByIdAsync(
                    randomExamAttachment.ExamId,
                    randomExamAttachment.AttachmentId))
                        .ReturnsAsync(randomExamAttachment);

            // when
            ExamAttachment actualExamAttachment =
                await this.examAttachmentService.RetrieveExamAttachmentByIdAsync(
                    randomExamAttachment.ExamId,
                    randomExamAttachment.AttachmentId);

            // then
            actualExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(
                    randomExamAttachment.ExamId,
                    randomExamAttachment.AttachmentId),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
