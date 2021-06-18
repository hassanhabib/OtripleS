//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.Foundations.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllCourseAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedCourseAttachmentDependencyException =
                new CourseAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourseAttachments())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<CourseAttachmentDependencyException>(() =>
                this.courseAttachmentService.RetrieveAllCourseAttachments());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourseAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseAttachmentDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllCourseAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedCourseAttachmentServiceException =
                new CourseAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourseAttachments())
                    .Throws(exception);

            // when . then
            Assert.Throws<CourseAttachmentServiceException>(() =>
                this.courseAttachmentService.RetrieveAllCourseAttachments());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourseAttachments(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
