// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid randomStudentId = default;
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> actualStudentSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() => actualStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenSemesterCourseIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = default;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                parameterValue: inputSemesterCourseId);

            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            // when
            ValueTask<StudentSemesterCourse> actualStudentSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync(inputStudentId, inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() => actualStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageStudentSemesterCourseIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(randomDateTime);
            Guid inputSemasterCourseId = randomStudentSemesterCourse.SemesterCourseId;
            Guid inputStudentId = randomStudentSemesterCourse.StudentId;
            StudentSemesterCourse nullStorageStudentSemesterCourse = null;

            var notFoundStudentSemesterCourseException =
                new NotFoundStudentSemesterCourseException(inputStudentId, inputSemasterCourseId);

            var expectedSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(notFoundStudentSemesterCourseException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentSemesterCourseByIdAsync(inputStudentId, inputSemasterCourseId))
                    .ReturnsAsync(nullStorageStudentSemesterCourse);

            // when
            ValueTask<StudentSemesterCourse> actualStudentSemesterCourseRetrieveTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync(inputStudentId, inputSemasterCourseId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                actualStudentSemesterCourseRetrieveTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
