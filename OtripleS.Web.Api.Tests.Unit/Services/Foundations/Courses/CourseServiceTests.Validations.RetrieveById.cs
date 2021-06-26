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

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomCourseId = default;
            Guid inputCourseId = randomCourseId;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.Id),
                parameterValue: inputCourseId);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            //when
            ValueTask<Course> retrieveCourseByIdTask =
                this.courseService.RetrieveCourseByIdAsync(inputCourseId);

            //then
            await Assert.ThrowsAsync<CourseValidationException>(() => retrieveCourseByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageCourseIsNullAndLogItAsync()
        {
            //given
            Guid randomCourseId = Guid.NewGuid();
            Guid someCourseId = randomCourseId;
            Course invalidStorageCourse = null;
            var notFoundCourseException = new NotFoundCourseException(someCourseId);

            var expectedCourseValidationException =
                new CourseValidationException(notFoundCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(invalidStorageCourse);

            //when
            ValueTask<Course> retrieveCourseByIdTask =
                this.courseService.RetrieveCourseByIdAsync(someCourseId);

            //then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                retrieveCourseByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}