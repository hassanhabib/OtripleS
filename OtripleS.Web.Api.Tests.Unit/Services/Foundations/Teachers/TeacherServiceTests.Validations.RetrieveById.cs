// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Teachers;
using OtripleS.Web.Api.Models.Teachers.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Teachers
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomTeacherId = default;
            Guid inputTeacherId = randomTeacherId;

            var invalidTeacherException = new InvalidTeacherException(
                parameterName: nameof(Teacher.Id),
                parameterValue: inputTeacherId);

            var expectedTeacherValidationException =
                new TeacherValidationException(invalidTeacherException);

            // when
            ValueTask<Teacher> actualTeacherTask =
                this.teacherService.RetrieveTeacherByIdAsync(inputTeacherId);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() => actualTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetriveWhenStorageTeacherIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Teacher randomTeacher = CreateRandomTeacher(dateTime);
            Guid inputTeacherId = randomTeacher.Id;
            Teacher inputTeacher = randomTeacher;
            Teacher nullStorageTeacher = null;

            var notFoundTeacherException = new NotFoundTeacherException(inputTeacherId);

            var expectedTeacherValidationException =
                new TeacherValidationException(notFoundTeacherException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId))
                    .ReturnsAsync(nullStorageTeacher);

            // when
            ValueTask<Teacher> actualTeacherTask =
                this.teacherService.RetrieveTeacherByIdAsync(inputTeacherId);

            // then
            await Assert.ThrowsAsync<TeacherValidationException>(() => actualTeacherTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherByIdAsync(inputTeacherId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
