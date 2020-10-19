//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherContactIsNullAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = default;
            TeacherContact nullTeacherContact = randomTeacherContact;
            var nullTeacherContactException = new NullTeacherContactException();

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(nullTeacherContactException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(nullTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(It.IsAny<TeacherContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherIdIsInvalidAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            inputTeacherContact.TeacherId = default;

            var invalidTeacherContactInputException = new InvalidTeacherContactInputException(
                parameterName: nameof(TeacherContact.TeacherId),
                parameterValue: inputTeacherContact.TeacherId);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactInputException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(It.IsAny<TeacherContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            inputTeacherContact.ContactId = default;

            var invalidTeacherContactInputException = new InvalidTeacherContactInputException(
                parameterName: nameof(TeacherContact.ContactId),
                parameterValue: inputTeacherContact.ContactId);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactInputException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(It.IsAny<TeacherContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenTeacherContactAlreadyExistsAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact alreadyExistsTeacherContact = randomTeacherContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsTeacherContactException =
                new AlreadyExistsTeacherContactException(duplicateKeyException);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(alreadyExistsTeacherContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherContactAsync(alreadyExistsTeacherContact))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(alreadyExistsTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(alreadyExistsTeacherContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenReferneceExceptionAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact invalidTeacherContact = randomTeacherContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var foreignKeyConstraintConflictException = new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidTeacherContactReferenceException =
                new InvalidTeacherContactReferenceException(foreignKeyConstraintConflictException);

            var expectedTeacherContactValidationException =
                new TeacherContactValidationException(invalidTeacherContactReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherContactAsync(invalidTeacherContact))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(invalidTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(invalidTeacherContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}