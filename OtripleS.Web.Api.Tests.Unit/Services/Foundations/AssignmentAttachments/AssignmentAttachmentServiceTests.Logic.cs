// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldAddAttachmentAttachmentAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment inputAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment storageAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = storageAssignmentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAssignmentAttachmentAsync(inputAssignmentAttachment))
                    .ReturnsAsync(storageAssignmentAttachment);

            // when
            AssignmentAttachment actualAttachmentAttachment =
                await this.assignmentAttachmentService.AddAssignmentAttachmentAsync(inputAssignmentAttachment);

            // then
            actualAttachmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(inputAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveAssignmentAttachmentByIdAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            AssignmentAttachment storageAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = storageAssignmentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(
                    randomAssignmentAttachment.AssignmentId,
                    randomAssignmentAttachment.AttachmentId))
                        .ReturnsAsync(randomAssignmentAttachment);

            // when
            AssignmentAttachment actualAssignmentAttachment = await
                this.assignmentAttachmentService.RetrieveAssignmentAttachmentByIdAsync(
                    randomAssignmentAttachment.AssignmentId,
                    randomAssignmentAttachment.AttachmentId);

            // then
            actualAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(
                    randomAssignmentAttachment.AssignmentId,
                    randomAssignmentAttachment.AttachmentId),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllAssignmentAttachments()
        {
            // given
            IQueryable<AssignmentAttachment> randomAssignmentAttachments =
                CreateRandomAssignmentAttachments();

            IQueryable<AssignmentAttachment> storageAssignmentAttachments =
                randomAssignmentAttachments;

            IQueryable<AssignmentAttachment> expectedAssignmentAttachments =
                storageAssignmentAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAssignmentAttachments())
                    .Returns(storageAssignmentAttachments);

            // when
            IQueryable<AssignmentAttachment> actualAssignmentAttachments =
                this.assignmentAttachmentService.RetrieveAllAssignmentAttachments();

            // then
            actualAssignmentAttachments.Should().BeEquivalentTo(expectedAssignmentAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAssignmentAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRemoveAssignmentAttachmentAsync()
        {
            // given
            var randomAssignmentId = Guid.NewGuid();
            var randomAttachmentId = Guid.NewGuid();
            Guid inputAssignmentId = randomAssignmentId;
            Guid inputAttachmentId = randomAttachmentId;
            AssignmentAttachment randomAssignmentAttachment = CreateRandomAssignmentAttachment();
            randomAssignmentAttachment.AssignmentId = inputAssignmentId;
            randomAssignmentAttachment.AttachmentId = inputAttachmentId;
            AssignmentAttachment storageAssignmentAttachment = randomAssignmentAttachment;
            AssignmentAttachment expectedAssignmentAttachment = storageAssignmentAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId))
                    .ReturnsAsync(storageAssignmentAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAssignmentAttachmentAsync(storageAssignmentAttachment))
                    .ReturnsAsync(expectedAssignmentAttachment);

            // when
            AssignmentAttachment actualAssignmentAttachment =
                await this.assignmentAttachmentService.RemoveAssignmentAttachmentByIdAsync(
                    inputAssignmentId,
                    inputAttachmentId);

            // then
            actualAssignmentAttachment.Should().BeEquivalentTo(expectedAssignmentAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAssignmentAttachmentByIdAsync(inputAssignmentId, inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAssignmentAttachmentAsync(storageAssignmentAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
