// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldRemoveStudentAttachmentAsync()
        {
            // given
            var randomStudentId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputAttachmentId = randomAttachmentId;
            DateTimeOffset inputDateTime = GetRandomDateTime();
            StudentAttachment randomStudentAttachment = CreateRandomStudentAttachment(inputDateTime);
            randomStudentAttachment.StudentId = inputStudentId;
            randomStudentAttachment.AttachmentId = inputAttachmentId;
            StudentAttachment storageStudentAttachment = randomStudentAttachment;
            StudentAttachment expectedStudentAttachment = storageStudentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId))
                    .ReturnsAsync(storageStudentAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentAttachmentAsync(storageStudentAttachment))
                    .ReturnsAsync(expectedStudentAttachment);

            // when
            StudentAttachment actualStudentAttachment =
                await this.studentAttachmentService.RemoveStudentAttachmentByIdAsync(
                    inputStudentId, inputAttachmentId);

            // then
            actualStudentAttachment.Should().BeEquivalentTo(expectedStudentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentAttachmentByIdAsync(inputStudentId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAttachmentAsync(storageStudentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
