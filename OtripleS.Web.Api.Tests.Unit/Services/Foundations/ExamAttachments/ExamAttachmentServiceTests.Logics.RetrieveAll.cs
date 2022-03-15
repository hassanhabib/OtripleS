// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
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

    }
}
