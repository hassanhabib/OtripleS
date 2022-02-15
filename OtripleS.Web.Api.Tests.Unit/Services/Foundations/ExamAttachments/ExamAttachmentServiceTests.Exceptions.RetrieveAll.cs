//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.ExamAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamAttachments
{
    public partial class ExamAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllExamAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedExamAttachmentDependencyException =
                new ExamAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllExamAttachmentsAction = () =>
                this.examAttachmentService.RetrieveAllExamAttachments();

            // then
            Assert.Throws<ExamAttachmentDependencyException>(
                retrieveAllExamAttachmentsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedExamAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllExamAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedExamAttachmentServiceException =
                new ExamAttachmentServiceException(exception);
          
            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamAttachments())
                    .Throws(exception);

            // when
            Action retrieveAllExamAttachmentsAction = () =>
                this.examAttachmentService.RetrieveAllExamAttachments();

            // then
            Assert.Throws<ExamAttachmentServiceException>(
                retrieveAllExamAttachmentsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
