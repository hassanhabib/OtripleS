//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllTeacherAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();
            var expectedTeacherAttachmentDependencyException = new TeacherAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeacherAttachments())
                    .Throws(sqlException);

            // when
            Action retrieveAllTeacherAttachmentAction = () =>
                this.teacherAttachmentService.RetrieveAllTeacherAttachments();

            // then
            Assert.Throws<TeacherAttachmentDependencyException>(
                retrieveAllTeacherAttachmentAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeacherAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllTeacherAttachmentsWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();
            var expectedTeacherAttachmentServiceException = new TeacherAttachmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeacherAttachments())
                    .Throws(serviceException);

            // when
            Action retrieveAllTeacherAttachmentAction = () =>
                this.teacherAttachmentService.RetrieveAllTeacherAttachments();

            // then
            Assert.Throws<TeacherAttachmentServiceException>(
                retrieveAllTeacherAttachmentAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherAttachmentServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeacherAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
