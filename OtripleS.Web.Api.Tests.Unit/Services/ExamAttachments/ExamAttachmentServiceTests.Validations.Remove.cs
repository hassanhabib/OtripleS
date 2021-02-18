// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenExamIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomAttachmentId = Guid.NewGuid();
            Guid randomExamId = default;
            Guid inputAttachmentId = randomAttachmentId;
            Guid inputExamId = randomExamId;

            var invalidExamAttachmentInputException = new InvalidExamAttachmentException(
                parameterName: nameof(ExamAttachment.ExamId),
                parameterValue: inputExamId);

            var expectedExamAttachmentValidationException =
                new ExamAttachmentValidationException(invalidExamAttachmentInputException);

            // when
            ValueTask<ExamAttachment> removeExamAttachmentTask =
                this.ExamAttachmentService.RemoveExamAttachmentByIdAsync(inputExamId, inputAttachmentId);

            // then
            await Assert.ThrowsAsync<ExamAttachmentValidationException>(() =>
                removeExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamAttachmentByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAttachmentAsync(It.IsAny<ExamAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
