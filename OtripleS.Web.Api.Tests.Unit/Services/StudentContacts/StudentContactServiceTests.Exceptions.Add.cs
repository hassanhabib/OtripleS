//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            var sqlException = GetSqlException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentContactAsync(inputStudentContact))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentContact> addStudentContactTask =
                this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                addStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(inputStudentContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentContactDependencyException =
                new StudentContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentContactAsync(inputStudentContact))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentContact> addStudentContactTask =
                this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                addStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(inputStudentContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            var exception = new Exception();

            var expectedStudentContactServiceException =
                new StudentContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentContactAsync(inputStudentContact))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentContact> addStudentContactTask =
                 this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            await Assert.ThrowsAsync<StudentContactServiceException>(() =>
                addStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentContactServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(inputStudentContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
