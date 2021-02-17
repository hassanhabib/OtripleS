//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            ExamAttachment randomExamAttachment = CreateRandomExamAttachment();
            ExamAttachment someExamAttachment = randomExamAttachment;
            var sqlException = GetSqlException();

            var expectedExamAttachmentDependencyException =
                new ExamAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamAttachmentAsync(someExamAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ExamAttachment> addExamAttachmentTask =
                this.examAttachmentService.AddExamAttachmentAsync(someExamAttachment);

            // then
            await Assert.ThrowsAsync<ExamAttachmentDependencyException>(() =>
                addExamAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedExamAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAttachmentAsync(someExamAttachment),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
