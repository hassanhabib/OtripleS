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

            var invalidSemesterCourseException = new InvalidSemesterCourseException(
                parameterName: nameof(SemesterCourse.Id),
                parameterValue: invalidSemesterCourse.Id);

            var expectedSemesterCourseValidationException =
                new SemesterCourseValidationException(invalidSemesterCourseException);

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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
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

            var invalidSemesterCourseInputException = new InvalidSemesterCourseException(
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
    }
}
