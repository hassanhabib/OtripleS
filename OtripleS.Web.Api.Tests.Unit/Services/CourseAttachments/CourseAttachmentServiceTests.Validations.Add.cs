using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseAttachments
{
    public partial class CourseAttachmentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCourseAttachmentIsNullAndLogItAsync()
        {
            // given
            CourseAttachment nullCourseAttachment = default;
            var nullCourseAttachmentException = new NullCourseAttachmentException();

            var expectedCourseAttachmentValidationException =
                new CourseAttachmentValidationException(nullCourseAttachmentException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(nullCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentValidationException>(() =>
                addCourseAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseAttachmentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(It.IsAny<CourseAttachment>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
