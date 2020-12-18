// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
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

	}
}
