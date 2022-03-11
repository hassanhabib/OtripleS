using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
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
                await this.examAttachmentService.RemoveExamAttachmentByIdAsync(
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
