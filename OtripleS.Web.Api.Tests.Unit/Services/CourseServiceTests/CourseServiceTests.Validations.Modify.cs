// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------


using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using OtripleS.Web.Api.Models.Students.Exceptions;
using OtripleS.Web.Api.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseServiceTests
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCourseIsNullAndLogItAsync()
        {
            //given
            Course invalidCourse = null;

            var nullCourseException = new NullCourseException();
            var expectedCourseValidationException = 
                new CourseValidationException(nullCourseException);

            //when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            //then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCourseIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidCourseId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course invalidCourse = randomCourse;
            invalidCourse.Id = invalidCourseId;

            var invalidCourseException = new InvalidCourseInputException(
                parameterName: nameof(Course.Id),
                parameterValue: invalidCourse.Id);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            //when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            //then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCourseNameIsInvalidAndLogItAsync(
            string invalidCourseName)
        {
            // given
            Course randomCourse = CreateRandomCourse(DateTime.Now);
            Course invalidCourse = randomCourse;
            invalidCourse.Name = invalidCourseName;

            var invalidCourseException = new InvalidCourseInputException(
               parameterName: nameof(Course.Name),
               parameterValue: invalidCourse.Name);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCourseDescriptionIsInvalidAndLogItAsync(
            string invalidCourseDescription)
        {
            // given
            Course randomCourse = CreateRandomCourse(DateTime.Now);
            Course invalidCourse = randomCourse;
            invalidCourse.Description = invalidCourseDescription;

            var invalidCourseException = new InvalidCourseInputException(
               parameterName: nameof(Course.Description),
               parameterValue: invalidCourse.Description);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.CreatedBy = default;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Teacher.CreatedBy),
                parameterValue: inputCourse.CreatedBy);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedBy = default;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Teacher.UpdatedBy),
                parameterValue: inputCourse.UpdatedBy);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
