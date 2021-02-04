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
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            CourseAttachment someCourseAttachment = CreateRandomCourseAttachment();
            var sqlException = GetSqlException();

            var expectedCourseAttachmentDependencyException =
                new CourseAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAttachmentAsync(someCourseAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CourseAttachment> addCourseAttachmentTask =
                this.courseAttachmentService.AddCourseAttachmentAsync(someCourseAttachment);

            // then
            await Assert.ThrowsAsync<CourseAttachmentDependencyException>(() =>
                addCourseAttachmentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAttachmentAsync(someCourseAttachment),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
