// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenSemesterCourseIsNullAndLogItAsync()
        {
            //given
            StudentSemesterCourse invalidStudentSemesterCourse = null;
            var nullStudentSemesterCourseException = new NullStudentSemesterCourseException();

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(nullStudentSemesterCourseException);

            //when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(invalidStudentSemesterCourse);

            //then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidStudentId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse invalidStudentSemesterCourse = randomStudentSemesterCourse;
            invalidStudentSemesterCourse.StudentId = invalidStudentId;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.StudentId),
                parameterValue: invalidStudentSemesterCourse.StudentId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            //when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(invalidStudentSemesterCourse);

            //then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse invalidStudentSemesterCourse = randomStudentSemesterCourse;
            invalidStudentSemesterCourse.SemesterCourseId = invalidSemesterCourseId;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                parameterValue: invalidStudentSemesterCourse.SemesterCourseId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            //when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(invalidStudentSemesterCourse);

            //then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentSemesterCourseGradeIsInvalidAndLogItAsync(
                    string invalidStudentSemesterCourseGrade)
        {
            // given
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(DateTime.Now);
            StudentSemesterCourse invalidStudentSemesterCourse = randomStudentSemesterCourse;
            invalidStudentSemesterCourse.Grade = invalidStudentSemesterCourseGrade;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
               parameterName: nameof(StudentSemesterCourse.Grade),
               parameterValue: invalidStudentSemesterCourse.Grade);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(invalidStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenRepeatsInvalidAndLogItAsync()
        {
            //given
            int invalidRepeats = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse invalidStudentSemesterCourse = randomStudentSemesterCourse;
            invalidStudentSemesterCourse.Repeats = invalidRepeats;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.Repeats),
                parameterValue: invalidStudentSemesterCourse.Repeats);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            //when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(invalidStudentSemesterCourse);

            //then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.CreatedBy = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.CreatedBy),
                parameterValue: inputStudentSemesterCourse.CreatedBy);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.UpdatedBy),
                parameterValue: inputStudentSemesterCourse.UpdatedBy);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.CreatedDate = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.CreatedDate),
                parameterValue: inputStudentSemesterCourse.CreatedDate);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedDate = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                parameterValue: inputStudentSemesterCourse.UpdatedDate);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                parameterValue: inputStudentSemesterCourse.UpdatedDate);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = inputStudentSemesterCourse.CreatedBy;
            inputStudentSemesterCourse.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                parameterValue: inputStudentSemesterCourse.UpdatedDate);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStudentSemesterCourseDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse nonExistentStudentSemesterCourse = randomStudentSemesterCourse;
            nonExistentStudentSemesterCourse.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            StudentSemesterCourse noStudentSemesterCourse = null;
            var notFoundStudentSemesterCourseException = new NotFoundStudentSemesterCourseException
                (nonExistentStudentSemesterCourse.StudentId, nonExistentStudentSemesterCourse.SemesterCourseId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(notFoundStudentSemesterCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(
                    nonExistentStudentSemesterCourse.StudentId, nonExistentStudentSemesterCourse.SemesterCourseId))
                    .ReturnsAsync(noStudentSemesterCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(nonExistentStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync
                (nonExistentStudentSemesterCourse.StudentId, nonExistentStudentSemesterCourse.SemesterCourseId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(randomDate);
            StudentSemesterCourse invalidStudentSemesterCourse = randomStudentSemesterCourse;
            invalidStudentSemesterCourse.UpdatedDate = randomDate;
            StudentSemesterCourse storageStudentSemesterCourse = randomStudentSemesterCourse.DeepClone();
            Guid studentId = invalidStudentSemesterCourse.StudentId;
            Guid semesterCourseId = invalidStudentSemesterCourse.SemesterCourseId;
            invalidStudentSemesterCourse.CreatedDate = storageStudentSemesterCourse.CreatedDate.AddMinutes(randomNumber);

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.CreatedDate),
                parameterValue: invalidStudentSemesterCourse.CreatedDate);

            var expectedStudentSemesterCourseValidationException =
              new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(studentId, semesterCourseId))
                    .ReturnsAsync(storageStudentSemesterCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<StudentSemesterCourse> modifyStudentSemesterCourseTask =
                this.studentSemesterCourseService.ModifyStudentSemesterCourseAsync(invalidStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                modifyStudentSemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(
                    invalidStudentSemesterCourse.StudentId,
                    invalidStudentSemesterCourse.SemesterCourseId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
