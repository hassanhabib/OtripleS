//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentAttachmentAsync()
        {
            // given
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment();
            StudentAttachment inputStudentAttachment = randomStudentAttachment;
            StudentAttachment storageStudentAttachment = randomStudentAttachment;
            StudentAttachment expectedStudentAttachment = storageStudentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment))
                    .ReturnsAsync(storageStudentAttachment);

            // when
            StudentAttachment actualStudentAttachment =
                await this.studentAttachmentService.AddStudentAttachmentAsync(inputStudentAttachment);

            // then
            actualStudentAttachment.Should().BeEquivalentTo(expectedStudentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAttachmentAsync(inputStudentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllStudentAttachments()
        {
            // given
            IQueryable<StudentAttachment> randomStudentAttachments = CreateRandomStudentAttachments();
            IQueryable<StudentAttachment> storageStudentAttachments = randomStudentAttachments;
            IQueryable<StudentAttachment> expectedStudentAttachments = storageStudentAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Returns(storageStudentAttachments);

            // when
            IQueryable<StudentAttachment> actualStudentAttachments =
                this.studentAttachmentService.RetrieveAllStudentAttachments();

            // then
            actualStudentAttachments.Should().BeEquivalentTo(expectedStudentAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
                    .Returns(new ValueTask<StudentAttachment>(randomStudentAttachment));

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
