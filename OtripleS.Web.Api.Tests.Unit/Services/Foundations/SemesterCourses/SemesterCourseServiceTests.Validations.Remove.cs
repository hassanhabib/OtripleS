// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = default;
            Guid inputSemesterCourseId = randomSemesterCourseId;

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.Id),
                parameterValue: inputSemesterCourseId);
            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseInputException);

            //when
            ValueTask<SemesterCourse> actualSemesterCourseDeleteTask =
                this.semesterCourseService.RemoveSemesterCourseByIdAsync(inputSemesterCourseId);

            //then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(
                () => actualSemesterCourseDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSemesterCourseAsync(It.IsAny<SemesterCourse>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStorageSemesterCourseIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDateTime);
            Guid inputSemesterCourseId = randomSemesterCourse.Id;
            SemesterCourse nullStorageSemesterCourse = null;

            var notFoundSemesterCourseException = new NotFoundSemesterCourseException(inputSemesterCourseId);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(notFoundSemesterCourseException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ReturnsAsync(nullStorageSemesterCourse);

            // when
            ValueTask<SemesterCourse> actualSemesterCourseDeleteTask =
                this.semesterCourseService.RemoveSemesterCourseByIdAsync(inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseValidationException>(() => actualSemesterCourseDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteSemesterCourseAsync(It.IsAny<SemesterCourse>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}