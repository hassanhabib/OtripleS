//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRemoveExamAttachmentAsync()
        {
            // given
            var randomExamId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            Guid inputAttachmentId = randomAttachmentId;
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            randomExamAttachment.ExamId = inputExamId;
            randomExamAttachment.AttachmentId = inputAttachmentId;
            ExamAttachment storageExamAttachment = randomExamAttachment;
            ExamAttachment expectedExamAttachment = storageExamAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamAttachmentByIdAsync(inputExamId, inputAttachmentId))
                    .ReturnsAsync(storageExamAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteExamAttachmentAsync(storageExamAttachment))
                    .ReturnsAsync(expectedExamAttachment);

            // when
            ExamAttachment actualExamAttachment =
                await this.ExamAttachmentService.RemoveExamAttachmentByIdAsync(
                    inputExamId, inputAttachmentId);

            // then
            actualExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(inputExamId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(storageExamAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
