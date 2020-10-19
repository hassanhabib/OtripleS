//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            var sqlException = GetSqlException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactDependencyException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactDependencyException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            var exception = new Exception();

            var expectedTeacherContactServiceException =
                new TeacherContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact))
                    .ThrowsAsync(exception);

            // when
            ValueTask<TeacherContact> addTeacherContactTask =
                 this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

            // then
            await Assert.ThrowsAsync<TeacherContactServiceException>(() =>
                addTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
