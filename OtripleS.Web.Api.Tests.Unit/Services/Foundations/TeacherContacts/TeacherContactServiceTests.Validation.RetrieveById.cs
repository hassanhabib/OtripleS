// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenTeacherIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidContactId = Guid.Empty;
            Guid invalidTeacherId = Guid.Empty;

            var invalidTeacherContactException = new InvalidTeacherContactException(
                parameterName: nameof(TeacherContact.TeacherId),
                parameterValue: invalidTeacherId);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(invalidTeacherId, invalidContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() => retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherContactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidContactId = Guid.Empty;
            Guid invalidTeacherId = Guid.Empty;

            var invalidTeacherContactException = new InvalidTeacherContactException(
                parameterName: nameof(TeacherContact.ContactId),
                parameterValue: invalidContactId);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(invalidTeacherId, invalidContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() => retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTeacherContactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveWhenStorageTeacherContactIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            TeacherContact randomTeacherContact = CreateRandomTeacherContact(randomDateTime);
            Guid invalidContactId = Guid.Empty;
            Guid invalidTeacherId = Guid.Empty;
            TeacherContact nullStorageTeacherContact = null;

            var notFoundTeacherContactException =
                new NotFoundTeacherContactException(invalidTeacherId, invalidContactId);

            var expectedSemesterCourseValidationException =
                new TeacherContactValidationException(notFoundTeacherContactException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherContactByIdAsync(invalidTeacherId, invalidContactId))
                    .ReturnsAsync(nullStorageTeacherContact);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(invalidTeacherId, invalidContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}