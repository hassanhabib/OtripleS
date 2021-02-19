//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenExamAttachmentsWereEmptyAndLogIt()
        {
            // given
            IQueryable<ExamAttachment> emptyStorageExamAttachments =
                new List<ExamAttachment>().AsQueryable();

            IQueryable<ExamAttachment> storageExamAttachments =
                emptyStorageExamAttachments;

            IQueryable<ExamAttachment> expectedExamAttachments =
                emptyStorageExamAttachments;

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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No exam attachments found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
