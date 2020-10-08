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
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenStudentIdIsInvalidAndLogItAsync()
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
            ValueTask<StudentContact> actualStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() => actualStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnRetrieveWhenContactIdIsInvalidAndLogItAsync()
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
            ValueTask<StudentContact> actualStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync(inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() => actualStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
