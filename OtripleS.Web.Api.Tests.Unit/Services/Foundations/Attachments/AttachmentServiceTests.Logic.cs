﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attachments
{
    public partial class AttachmentServiceTests
    {
        [Fact]
        public async Task ShouldInsertAttachmentAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;

            Attachment randomAttachment =
                CreateRandomAttachment(randomDateTime);

            randomAttachment.UpdatedBy = randomAttachment.CreatedBy;
            randomAttachment.UpdatedDate = randomAttachment.CreatedDate;
            Attachment inputAttachment = randomAttachment;
            Attachment storageAttachment = randomAttachment;
            Attachment expectedAttachment = storageAttachment;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttachmentAsync(inputAttachment))
                    .ReturnsAsync(storageAttachment);

            // when
            Attachment actualAttachment =
                await this.attachmentService.AddAttachmentAsync(inputAttachment);

            // then
            actualAttachment.Should().BeEquivalentTo(expectedAttachment);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttachmentAsync(inputAttachment),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }        

        [Fact]
        public void ShouldRetrieveAllAttachments()
        {
            // given
            IQueryable<Attachment> randomAttachments = CreateRandomAttachments();
            IQueryable<Attachment> storageAttachments = randomAttachments;
            IQueryable<Attachment> expectedAttachments = storageAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttachments())
                    .Returns(storageAttachments);

            // when
            IQueryable<Attachment> actualAttachments =
                this.attachmentService.RetrieveAllAttachments();

            // then
            actualAttachments.Should().BeEquivalentTo(expectedAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }        

        [Fact]
        public async Task ShouldRemoveAttachmentAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();

            Attachment randomAttachment =
                CreateRandomAttachment(dates: dateTime);

            Guid inputAttachmentId = randomAttachment.Id;
            Attachment inputAttachment = randomAttachment;
            Attachment storageAttachment = inputAttachment;
            Attachment expectedAttachment = storageAttachment;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId))
                    .ReturnsAsync(inputAttachment);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAttachmentAsync(inputAttachment))
                    .ReturnsAsync(storageAttachment);

            // when
            Attachment actualAttachment =
                await this.attachmentService.RemoveAttachmentByIdAsync(inputAttachmentId);

            // then
            actualAttachment.Should().BeEquivalentTo(expectedAttachment);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(inputAttachmentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttachmentAsync(inputAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
