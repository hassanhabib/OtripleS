// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCourseId = default;
            Guid inputCourseId = randomCourseId;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.Id),
                parameterValue: inputCourseId);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            // when
            ValueTask<Course> actualCourseTask =
                this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() => actualCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAsync(It.IsAny<Course>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageCourseIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Guid inputCourseId = randomCourse.Id;
            Course inputCourse = randomCourse;
            Course nullStorageCourse = null;

            var notFoundCourseException = new NotFoundCourseException(inputCourseId);

            var expectedCourseValidationException =
                new CourseValidationException(notFoundCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ReturnsAsync(nullStorageCourse);

            // when
            ValueTask<Course> actualCourseTask =
                this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() => actualCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAsync(It.IsAny<Course>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
