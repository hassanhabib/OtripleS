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
