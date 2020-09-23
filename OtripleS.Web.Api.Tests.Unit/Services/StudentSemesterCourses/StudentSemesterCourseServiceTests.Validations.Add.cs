//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentSemesterCourseIsNullAndLogItAsync()
        {
            // given
            StudentSemesterCourse randomStudentSemesterCourse = null;
            StudentSemesterCourse nullStudentSemesterCourse = randomStudentSemesterCourse;
            var nullStudentSemesterCourseException = new NullStudentSemesterCourseException();

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(nullStudentSemesterCourseException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(nullStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.StudentId = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.StudentId),
                parameterValue: inputStudentSemesterCourse.StudentId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> registerStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                registerStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenSemesterCourseIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.SemesterCourseId = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                parameterValue: inputStudentSemesterCourse.SemesterCourseId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> registerStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                registerStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.CreatedDate = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.CreatedDate),
                parameterValue: inputStudentSemesterCourse.CreatedDate);

            var expectedSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.CreatedBy = default;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.CreatedBy),
                parameterValue: inputStudentSemesterCourse.CreatedBy);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByInvalidAndLogItAsync()
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
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateInvalidAndLogItAsync()
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
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateInvalidAndLogItAsync()
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
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = Guid.NewGuid();

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.UpdatedBy),
                parameterValue: inputStudentSemesterCourse.UpdatedBy);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = randomStudentSemesterCourse.CreatedBy;
            inputStudentSemesterCourse.UpdatedDate = GetRandomDateTime();

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.UpdatedDate),
                parameterValue: inputStudentSemesterCourse.UpdatedDate);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = inputStudentSemesterCourse.CreatedBy;
            inputStudentSemesterCourse.CreatedDate = dateTime.AddMinutes(minutes);
            inputStudentSemesterCourse.UpdatedDate = inputStudentSemesterCourse.CreatedDate;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.CreatedDate),
                parameterValue: inputStudentSemesterCourse.CreatedDate);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnCreateWhenStudentSemesterCourseAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse alreadyExistsStudentSemesterCourse = randomStudentSemesterCourse;
            alreadyExistsStudentSemesterCourse.UpdatedBy = alreadyExistsStudentSemesterCourse.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsStudentSemesterCourseException =
                new AlreadyExistsStudentSemesterCourseException(duplicateKeyException);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(alreadyExistsStudentSemesterCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentSemesterCourseAsync(alreadyExistsStudentSemesterCourse))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(alreadyExistsStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                createStudentSemesterCourseTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentSemesterCourseAsync(alreadyExistsStudentSemesterCourse),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
