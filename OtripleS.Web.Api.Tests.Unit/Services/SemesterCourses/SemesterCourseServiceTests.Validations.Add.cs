// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourses
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.CreatedDate = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.CreatedDate),
                parameterValue: inputSemesterCourse.CreatedDate);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedDate = default;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.UpdatedDate),
                parameterValue: inputSemesterCourse.UpdatedDate);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = Guid.NewGuid();

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = randomSemesterCourse.CreatedBy;
            inputSemesterCourse.UpdatedDate = GetRandomDateTime();

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.UpdatedDate),
                parameterValue: inputSemesterCourse.UpdatedDate);

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

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = inputSemesterCourse.CreatedBy;
            inputSemesterCourse.CreatedDate = dateTime.AddMinutes(minutes);
            inputSemesterCourse.UpdatedDate = inputSemesterCourse.CreatedDate;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.CreatedDate),
                parameterValue: inputSemesterCourse.CreatedDate);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
        public async void ShouldThrowValidationExceptionOnCreateWhenSemesterCourseAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse alreadyExistsSemesterCourse = randomSemesterCourse;
            alreadyExistsSemesterCourse.UpdatedBy = alreadyExistsSemesterCourse.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsSemesterCourseException =
                new AlreadyExistsSemesterCourseException(duplicateKeyException);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(alreadyExistsSemesterCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSemesterCourseAsync(alreadyExistsSemesterCourse))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(alreadyExistsSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                createSemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSemesterCourseAsync(alreadyExistsSemesterCourse),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
