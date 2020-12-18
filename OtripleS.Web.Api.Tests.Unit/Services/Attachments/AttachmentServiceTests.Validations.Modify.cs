// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Attachments;
using OtripleS.Web.Api.Models.Attachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Attachments
{
	public partial class AttachmentServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenAttachmentIsNullAndLogItAsync()
		{
			//given
			Attachment invalidAttachment = null;
			var nullAttachmentException = new NullAttachmentException();

			var expectedAttachmentValidationException =
				new AttachmentValidationException(nullAttachmentException);

			//when
			ValueTask<Attachment> modifyAttachmentTask =
				this.attachmentService.ModifyAttachmentAsync(invalidAttachment);

			//then
			await Assert.ThrowsAsync<AttachmentValidationException>(() =>
				modifyAttachmentTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Attachment randomAttachment = CreateRandomAttachment(randomDateTime);
            Attachment someAttachment = randomAttachment;
            someAttachment.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedAttachmentDependencyException =
                new AttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttachmentByIdAsync(someAttachment.Id))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(someAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentDependencyException>(() =>
                modifyAttachmentTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttachmentByIdAsync(someAttachment.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenAttachmentLabelIsInvalidAndLogItAsync(
                   string invalidAttachmentLabel)
        {
            // given
            Attachment randomAttachment = CreateRandomAttachment(DateTime.Now);
            Attachment invalidAttachment = randomAttachment;
            invalidAttachment.Label = invalidAttachmentLabel;

            var invalidAttachmentException = new InvalidAttachmentException(
               parameterName: nameof(Attachment.Label),
               parameterValue: invalidAttachment.Label);

            var expectedAttachmentValidationException =
                new AttachmentValidationException(invalidAttachmentException);

            // when
            ValueTask<Attachment> modifyAttachmentTask =
                this.attachmentService.ModifyAttachmentAsync(invalidAttachment);

            // then
            await Assert.ThrowsAsync<AttachmentValidationException>(() =>
                modifyAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
