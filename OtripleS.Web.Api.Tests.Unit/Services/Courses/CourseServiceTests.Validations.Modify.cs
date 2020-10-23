// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Courses
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
                parameterName: nameof(Course.CreatedBy),
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
                parameterName: nameof(Course.UpdatedBy),
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.CreatedDate = default;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.CreatedDate),
                parameterValue: inputCourse.CreatedDate);

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedDate = default;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.UpdatedDate),
                parameterValue: inputCourse.UpdatedDate);

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.UpdatedDate),
                parameterValue: inputCourse.UpdatedDate);

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

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedBy = inputCourse.CreatedBy;
            inputCourse.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.UpdatedDate),
                parameterValue: inputCourse.UpdatedDate);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
        public async Task ShouldThrowValidationExceptionOnModifyIfCourseDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course nonExistentCourse = randomCourse;
            nonExistentCourse.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Course noCourse = null;
            var notFoundCourseException = new NotFoundCourseException(nonExistentCourse.Id);

            var expectedCourseValidationException =
                new CourseValidationException(notFoundCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(nonExistentCourse.Id))
                    .ReturnsAsync(noCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(nonExistentCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(nonExistentCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomDate);
            Course invalidCourse = randomCourse;
            invalidCourse.UpdatedDate = randomDate;
            Course storageCourse = randomCourse.DeepClone();
            Guid courseId = invalidCourse.Id;
            invalidCourse.CreatedDate = storageCourse.CreatedDate.AddMinutes(randomNumber);

            var invalidCourseException = new InvalidCourseInputException(
                parameterName: nameof(Course.CreatedDate),
                parameterValue: invalidCourse.CreatedDate);

            var expectedCourseValidationException =
              new CourseValidationException(invalidCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(courseId))
                    .ReturnsAsync(storageCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(invalidCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomDate);
            Course invalidCourse = randomCourse;
            invalidCourse.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Course storageCourse = randomCourse.DeepClone();
            Guid courseId = invalidCourse.Id;
            invalidCourse.CreatedBy = invalidCreatedBy;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.CreatedBy),
                parameterValue: invalidCourse.CreatedBy);

            var expectedCourseValidationException =
              new CourseValidationException(invalidCourseInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(courseId))
                    .ReturnsAsync(storageCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(invalidCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomDate);
            randomCourse.CreatedDate = randomCourse.CreatedDate.AddMinutes(minutesInThePast);
            Course invalidCourse = randomCourse;
            invalidCourse.UpdatedDate = randomDate;
            Course storageCourse = randomCourse.DeepClone();
            Guid courseId = invalidCourse.Id;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.UpdatedDate),
                parameterValue: invalidCourse.UpdatedDate);

            var expectedCourseValidationException =
              new CourseValidationException(invalidCourseInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(courseId))
                    .ReturnsAsync(storageCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                modifyCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(invalidCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
