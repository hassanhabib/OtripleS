// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllStudentAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedStudentAttachmentDependencyException =
                new StudentAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllStudentAttachmentsAction = () =>
                this.studentAttachmentService.RetrieveAllStudentAttachments();

            // then
            Assert.Throws<StudentAttachmentDependencyException>(
                retrieveAllStudentAttachmentsAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllStudentAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedStudentAttachmentServiceException =
                new FailedStudentAttachmentServiceException(serviceException);

            var expectedStudentAttachmentServiceException =
                new StudentAttachmentServiceException(failedStudentAttachmentServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllStudentAttachmentsAction = () =>
                this.studentAttachmentService.RetrieveAllStudentAttachments();

            // then
            Assert.Throws<StudentAttachmentServiceException>(
                retrieveAllStudentAttachmentsAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
