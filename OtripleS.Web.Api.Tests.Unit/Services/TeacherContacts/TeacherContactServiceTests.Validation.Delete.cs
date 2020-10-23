//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenTeacherIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid randomTeacherId = default;
            Guid inputContactId = randomContactId;
            Guid inputTeacherId = randomTeacherId;

            var invalidTeacherContactInputException = new InvalidTeacherContactInputException(
                parameterName: nameof(TeacherContact.TeacherId),
                parameterValue: inputTeacherId);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactInputException);

            // when
            ValueTask<TeacherContact> removeTeacherContactTask =
                this.teacherContactService.RemoveTeacherContactByIdAsync(inputTeacherId, inputContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() => removeTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherContactAsync(It.IsAny<TeacherContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = default;
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputTeacherId = randomTeacherId;

            var invalidTeacherContactInputException = new InvalidTeacherContactInputException(
                parameterName: nameof(TeacherContact.ContactId),
                parameterValue: inputContactId);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactInputException);

            // when
            ValueTask<TeacherContact> removeTeacherContactTask =
                this.teacherContactService.RemoveTeacherContactByIdAsync(inputTeacherId, inputContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() => removeTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherContactAsync(It.IsAny<TeacherContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageTeacherContactIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            TeacherContact randomTeacherContact = CreateRandomTeacherContact(randomDateTime);
            Guid inputContactId = randomTeacherContact.ContactId;
            Guid inputTeacherId = randomTeacherContact.TeacherId;
            TeacherContact nullStorageTeacherContact = null;

            var notFoundTeacherContactException =
                new NotFoundTeacherContactException(inputTeacherId, inputContactId);

            var expectedSemesterCourseValidationException =
                new TeacherContactValidationException(notFoundTeacherContactException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId))
                    .ReturnsAsync(nullStorageTeacherContact);

            // when
            ValueTask<TeacherContact> removeTeacherContactTask =
                this.teacherContactService.RemoveTeacherContactByIdAsync(inputTeacherId, inputContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                removeTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherContactAsync(It.IsAny<TeacherContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}