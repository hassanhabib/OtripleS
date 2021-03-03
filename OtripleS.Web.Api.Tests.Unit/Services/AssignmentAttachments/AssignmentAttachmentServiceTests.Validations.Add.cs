//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.AssignmentAttachments;
using OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AssignmentAttachments
{
    public partial class AssignmentAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenAssignmentAttachmentIsNullAndLogItAsync()
        {
            // given
            AssignmentAttachment randomAssignmentAttachment = default;
            AssignmentAttachment nullAssignmentAttachment = randomAssignmentAttachment;
            var nullAssignmentAttachmentException = new NullAssignmentAttachmentException();

            var expectedAssignmentAttachmentValidationException =
                new AssignmentAttachmentValidationException(nullAssignmentAttachmentException);

            // when
            ValueTask<AssignmentAttachment> addAssignmentAttachmentTask =
                this.assignmentAttachmentService.AddAssignmentAttachmentAsync(nullAssignmentAttachment);

            // then
            await Assert.ThrowsAsync<AssignmentAttachmentValidationException>(() =>
                addAssignmentAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAssignmentAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAssignmentAttachmentAsync(It.IsAny<AssignmentAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
