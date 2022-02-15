// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
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

        [Fact]
        public void ShouldRetrieveAllExamAttachments()
        {
            // given
            IQueryable<ExamAttachment> randomExamAttachments =
                CreateRandomExamAttachments();

            IQueryable<ExamAttachment> storageExamAttachments =
                randomExamAttachments;

            IQueryable<ExamAttachment> expectedExamAttachments =
                storageExamAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamAttachments())
                    .Returns(storageExamAttachments);

            // when
            IQueryable<ExamAttachment> actualExamAttachments =
                this.examAttachmentService.RetrieveAllExamAttachments();

            // then
            actualExamAttachments.Should().BeEquivalentTo(expectedExamAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
