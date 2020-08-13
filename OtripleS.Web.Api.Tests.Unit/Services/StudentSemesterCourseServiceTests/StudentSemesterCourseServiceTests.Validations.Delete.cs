using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourseServiceTests
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

            var invalidSemesterCourseInputException = new InvalidStudentSemesterCourseException(
                parameterName: nameof(StudentSemesterCourse.SemesterCourseId),
                parameterValue: inputSemesterCourseId
            );
            var expectedStudentSemesterCourseValidationException =
                new StudentSemesterCourseValidationException(invalidSemesterCourseInputException);

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
                broker.SelectStudentSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentSemesterCourseAsync(It.IsAny<StudentSemesterCourse>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}