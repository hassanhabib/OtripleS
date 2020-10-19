// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using OtripleS.Web.Api.Models.Courses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCourseIsNullAndLogItAsync()
        {
            // given
            Course randomCourse = null;
            Course nullCourse = randomCourse;

            var nullCourseException = new NullCourseException();

            var expectedCourseValidationException =
                new CourseValidationException(nullCourseException);

            // when
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(nullCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.Id = default;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.Id),
                parameterValue: inputCourse.Id);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            // when
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedBy = Guid.NewGuid();

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.UpdatedBy),
                parameterValue: inputCourse.UpdatedBy);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            // when
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedBy = randomCourse.CreatedBy;
            inputCourse.UpdatedDate = GetRandomDateTime();

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.UpdatedDate),
                parameterValue: inputCourse.UpdatedDate);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            // when
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedBy = inputCourse.CreatedBy;
            inputCourse.CreatedDate = dateTime.AddMinutes(minutes);
            inputCourse.UpdatedDate = inputCourse.CreatedDate;

            var invalidCourseInputException = new InvalidCourseInputException(
                parameterName: nameof(Course.CreatedDate),
                parameterValue: inputCourse.CreatedDate);

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(inputCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCourseAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course alreadyExistsCourse = randomCourse;
            alreadyExistsCourse.UpdatedBy = alreadyExistsCourse.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCourseException =
                new AlreadyExistsCourseException(duplicateKeyException);

            var expectedCourseValidationException =
                new CourseValidationException(alreadyExistsCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAsync(alreadyExistsCourse))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Course> createCourseTask =
                this.courseService.CreateCourseAsync(alreadyExistsCourse);

            // then
            await Assert.ThrowsAsync<CourseValidationException>(() =>
                createCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(alreadyExistsCourse),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
