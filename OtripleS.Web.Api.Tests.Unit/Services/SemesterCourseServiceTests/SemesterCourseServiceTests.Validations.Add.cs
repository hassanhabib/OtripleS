// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourseServiceTests
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenSemesterCourseIsNullAndLogItAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = null;
            SemesterCourse nullSemesterCourse = randomSemesterCourse;
            var nullSemesterCourseException = new NullSemesterCourseException();

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(nullSemesterCourseException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(nullSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
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
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.Id = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.Id),
                parameterValue: inputSemesterCourse.Id);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> registerSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                registerSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenStartDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.StartDate = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.StartDate),
                parameterValue: inputSemesterCourse.StartDate);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenEndDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.EndDate = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.EndDate),
                parameterValue: inputSemesterCourse.EndDate);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCourseIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.CourseId = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.CourseId),
                parameterValue: inputSemesterCourse.CourseId);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenTeacherIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.TeacherId = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.TeacherId),
                parameterValue: inputSemesterCourse.TeacherId);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenClassroomIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.ClassroomId = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.ClassroomId),
                parameterValue: inputSemesterCourse.ClassroomId);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.CreatedBy = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.CreatedBy),
                parameterValue: inputSemesterCourse.CreatedBy);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.UpdatedBy),
                parameterValue: inputSemesterCourse.UpdatedBy);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
