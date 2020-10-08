//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentContactIsNullAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = default;
            StudentContact nullStudentContact = randomStudentContact;
            var nullStudentContactException = new NullStudentContactException();

            var expectedStudentContactValidationException =
                new StudentContactValidationException(nullStudentContactException);

            // when
            ValueTask<StudentContact> addStudentContactTask =
                this.studentContactService.AddStudentContactAsync(nullStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() =>
                addStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentIdIsInvalidAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            inputStudentContact.StudentId = default;

            var invalidStudentContactInputException = new InvalidStudentContactInputException(
                parameterName: nameof(StudentContact.StudentId),
                parameterValue: inputStudentContact.StudentId);

            var expectedStudentContactValidationException =
                new StudentContactValidationException(invalidStudentContactInputException);

            // when
            ValueTask<StudentContact> addStudentContactTask =
                this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() =>
                addStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenContactIdIsInvalidAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            inputStudentContact.ContactId = default;

            var invalidStudentContactInputException = new InvalidStudentContactInputException(
                parameterName: nameof(StudentContact.ContactId),
                parameterValue: inputStudentContact.ContactId);

            var expectedStudentContactValidationException =
                new StudentContactValidationException(invalidStudentContactInputException);

            // when
            ValueTask<StudentContact> addStudentContactTask =
                this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() =>
                addStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(It.IsAny<StudentContact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentContactAlreadyExistsAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact alreadyExistsStudentContact = randomStudentContact;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsStudentContactException =
                new AlreadyExistsStudentContactException(duplicateKeyException);

            var expectedStudentContactValidationException =
                new StudentContactValidationException(alreadyExistsStudentContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentContactAsync(alreadyExistsStudentContact))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<StudentContact> createStudentContactTask =
                this.studentContactService.AddStudentContactAsync(alreadyExistsStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactValidationException>(() =>
                createStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedStudentContactValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(alreadyExistsStudentContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
