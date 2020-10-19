//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid randomStudentId = default;
            Guid inputContactId = randomContactId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentContactInputException = new InvalidStudentContactInputException(
                parameterName: nameof(StudentContact.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentContactValidationException =
                new StudentContactValidationException(invalidStudentContactInputException);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() => removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRemoveWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomContactId = default;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentContactInputException = new InvalidStudentContactInputException(
                parameterName: nameof(StudentContact.ContactId),
                parameterValue: inputContactId);

            var expectedStudentContactValidationException =
                new StudentContactValidationException(invalidStudentContactInputException);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() => removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenStorageStudentContactIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentContact randomStudentContact = CreateRandomStudentContact(randomDateTime);
            Guid inputContactId = randomStudentContact.ContactId;
            Guid inputStudentId = randomStudentContact.StudentId;
            StudentContact nullStorageStudentContact = null;

            var notFoundStudentContactException =
                new NotFoundStudentContactException(inputStudentId, inputContactId);

            var expectedSemesterCourseValidationException =
                new StudentContactValidationException(notFoundStudentContactException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ReturnsAsync(nullStorageStudentContact);

            // when
            ValueTask<StudentContact> removeStudentContactTask =
                this.studentContactService.RemoveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() =>
                removeStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}