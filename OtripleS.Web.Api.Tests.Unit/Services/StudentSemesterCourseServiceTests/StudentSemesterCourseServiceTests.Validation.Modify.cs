// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourseServiceTests
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

            var invalidStudentSemesterCourseException = new InvalidStudentSemesterCourseException(
                parameterName: nameof(StudentSemesterCourse.StudentId),
                parameterValue: invalidStudentSemesterCourse.StudentId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseException);

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

            var invalidStudentSemesterCourseException = new InvalidStudentSemesterCourseException(
                parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                parameterValue: invalidStudentSemesterCourse.SemesterCourseId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseException);

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

            var invalidStudentSemesterCourseException = new InvalidStudentSemesterCourseException(
               parameterName: nameof(StudentSemesterCourse.Grade),
               parameterValue: invalidStudentSemesterCourse.Grade);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseException);

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

            var invalidStudentSemesterCourseException = new InvalidStudentSemesterCourseException(
                parameterName: nameof(StudentSemesterCourse.Repeats),
                parameterValue: invalidStudentSemesterCourse.Repeats);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseException);

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

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseException(
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
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
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

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseException(
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
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
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

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseException(
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
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
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

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseException(
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
                broker.SelectAssignmentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
