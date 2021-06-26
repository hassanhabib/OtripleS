//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            CourseAttachment someCourseAttachment = CreateRandomCourseAttachment();
            var sqlException = GetSqlException();

            var expectedCourseAttachmentDependencyException =
                new CourseAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(someCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                addCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            CourseAttachment someCourseAttachment = CreateRandomCourseAttachment();
            var databaseUpdateException = new DbUpdateException();

            var expectedCourseAttachmentDependencyException =
                new CourseAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(someCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                addCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            CourseAttachment someCourseAttachment = CreateRandomCourseAttachment();
            var exception = new Exception();

            var expectedCourseAttachmentServiceException =
                new CourseAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                 this.courseAttachmentService.AddCourseAttachmentAsync(someCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentServiceException>(() =>
                addCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
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
