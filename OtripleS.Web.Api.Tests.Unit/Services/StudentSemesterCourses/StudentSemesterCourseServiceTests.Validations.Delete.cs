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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenSemesterCourseIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = default;
            Guid randomStudentId = default;
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                parameterValue: inputSemesterCourseId
            );
            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            //when
            ValueTask<StudentSemesterCourse> actualStudentSemesterCourseDeleteTask =
                this.studentSemesterCourseService.DeleteStudentSemesterCourseAsync(inputSemesterCourseId, inputStudentId);

            //then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(
                () => actualStudentSemesterCourseDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentSemesterCourseAsync(It.IsAny<StudentSemesterCourse>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid randomStudentId = default;
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentSemesterCourseInputException = new InvalidStudentSemesterCourseInputException(
                parameterName: nameof(StudentSemesterCourse.StudentId),
                parameterValue: inputStudentId
            );
            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidStudentSemesterCourseInputException);

            //when
            ValueTask<StudentSemesterCourse> actualStudentSemesterCourseDeleteTask =
                this.studentSemesterCourseService.DeleteStudentSemesterCourseAsync(inputSemesterCourseId, inputStudentId);

            //then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(
                () => actualStudentSemesterCourseDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentSemesterCourseAsync(It.IsAny<StudentSemesterCourse>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageStudentSemesterCourseIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(randomDateTime);
            Guid inputSemasterCourseId = randomStudentSemesterCourse.SemesterCourseId;
            Guid inputStudentId = randomStudentSemesterCourse.StudentId;
            StudentSemesterCourse nullStorageStudentSemesterCourse = null;

            var notFoundStudentSemesterCourseException =
                new NotFoundStudentSemesterCourseException(inputSemasterCourseId, inputStudentId);

            var expectedSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(notFoundStudentSemesterCourseException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentSemesterCourseByIdAsync(inputSemasterCourseId, inputStudentId))
                    .ReturnsAsync(nullStorageStudentSemesterCourse);

            // when
            ValueTask<StudentSemesterCourse> actualStudentSemesterCourseDeleteTask =
                this.studentSemesterCourseService.DeleteStudentSemesterCourseAsync(inputSemasterCourseId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseValidationException>(() =>
                actualStudentSemesterCourseDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputSemasterCourseId, inputStudentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentSemesterCourseAsync(It.IsAny<StudentSemesterCourse>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}