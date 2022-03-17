// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveStudentAttachmentById()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment storageStudentAttachment = randomStudentAttachment;
            StudentAttachment expectedStudentAttachment = storageStudentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync
                (randomStudentAttachment.StudentId, randomStudentAttachment.AttachmentId))
                    .ReturnsAsync(storageStudentAttachment);

            // when
            StudentAttachment actualStudentAttachment = await
                this.studentAttachmentService.RetrieveStudentAttachmentByIdAsync(
                    randomStudentAttachment.StudentId, randomStudentAttachment.AttachmentId);

            // then
            actualStudentAttachment.Should().BeEquivalentTo(expectedStudentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync
                (randomStudentAttachment.StudentId, randomStudentAttachment.AttachmentId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
