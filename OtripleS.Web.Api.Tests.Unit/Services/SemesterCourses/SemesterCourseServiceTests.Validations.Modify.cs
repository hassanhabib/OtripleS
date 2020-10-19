// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenSemesterCourseIsNullAndLogItAsync()
        {
            //given
            SemesterCourse invalidSemesterCourse = null;
            var nullSemesterCourseException = new NullSemesterCourseException();

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(nullSemesterCourseException);

            //when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(invalidSemesterCourse);

            //then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenSemesterCourseIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidSemesterCourseId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse invalidSemesterCourse = randomSemesterCourse;
            invalidSemesterCourse.Id = invalidSemesterCourseId;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.Id),
                parameterValue: invalidSemesterCourse.Id);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            //when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(invalidSemesterCourse);

            //then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenStartDateIsInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenEndDateIsInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenCourseIsInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenTeacherIsInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenClassroomIsInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateInvalidAndLogItAsync()
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
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = inputSemesterCourse.CreatedBy;
            inputSemesterCourse.CreatedDate = inputSemesterCourse.CreatedDate;
            inputSemesterCourse.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.UpdatedDate),
                parameterValue: inputSemesterCourse.UpdatedDate);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDate);
            SemesterCourse invalidSemesterCourse = randomSemesterCourse;
            invalidSemesterCourse.UpdatedDate = randomDate;
            SemesterCourse storageSemesterCourse = randomSemesterCourse.DeepClone();
            Guid semesterCourseId = invalidSemesterCourse.Id;
            invalidSemesterCourse.CreatedDate = storageSemesterCourse.CreatedDate.AddMinutes(randomNumber);

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.CreatedDate),
                parameterValue: invalidSemesterCourse.CreatedDate);

            var expectedSemesterCourseValidationException =
              new SemesterCourseValidationException(invalidSemesterCourseInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(semesterCourseId))
                    .ReturnsAsync(storageSemesterCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(invalidSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(invalidSemesterCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
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
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDate);
            randomSemesterCourse.CreatedDate = randomSemesterCourse.CreatedDate.AddMinutes(minutesInThePast);
            SemesterCourse invalidSemesterCourse = randomSemesterCourse;
            invalidSemesterCourse.UpdatedDate = randomDate;
            SemesterCourse storageSemesterCourse = randomSemesterCourse.DeepClone();
            Guid semesterCourseId = invalidSemesterCourse.Id;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.UpdatedDate),
                parameterValue: invalidSemesterCourse.UpdatedDate);

            var expectedSemesterCourseValidationException =
              new SemesterCourseValidationException(invalidSemesterCourseInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(semesterCourseId))
                    .ReturnsAsync(storageSemesterCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(invalidSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(invalidSemesterCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
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
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDate);
            SemesterCourse invalidSemesterCourse = randomSemesterCourse;
            invalidSemesterCourse.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            SemesterCourse storageSemesterCourse = randomSemesterCourse.DeepClone();
            Guid semesterCourseId = invalidSemesterCourse.Id;
            invalidSemesterCourse.CreatedBy = invalidCreatedBy;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseInputException(
                parameterName: nameof(SemesterCourse.CreatedBy),
                parameterValue: invalidSemesterCourse.CreatedBy);

            var expectedSemesterCourseValidationException =
              new SemesterCourseValidationException(invalidSemesterCourseInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(semesterCourseId))
                    .ReturnsAsync(storageSemesterCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<SemesterCourse> modifySemesterCourseTask =
                this.semesterCourseService.ModifySemesterCourseAsync(invalidSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() =>
                modifySemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(invalidSemesterCourse.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
