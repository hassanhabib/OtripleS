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
        public async Task ShouldAddExamAttachmentAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment inputExamAttachment = randomExamAttachment;
            ExamAttachment storageExamAttachment = randomExamAttachment;
            ExamAttachment expectedExamAttachment = storageExamAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamAttachmentAsync(inputExamAttachment))
                    .ReturnsAsync(storageExamAttachment);

            // when
            ExamAttachment actualExamAttachment =
                await this.examAttachmentService.AddExamAttachmentAsync(inputExamAttachment);

            // then
            actualExamAttachment.Should().BeEquivalentTo(expectedExamAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(inputExamAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
