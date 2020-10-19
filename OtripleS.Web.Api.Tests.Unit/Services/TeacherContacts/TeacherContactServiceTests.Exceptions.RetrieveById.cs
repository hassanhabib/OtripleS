// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var inputContactId = randomContactId;
            Guid randomTeacherId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            SqlException sqlException = GetSqlException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(
                    inputTeacherId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactDependencyException>(() =>
                retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomTeacherId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputTeacherId = randomTeacherId;
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync
                (inputTeacherId, inputContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactDependencyException>(
                () => retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            var randomContactId = Guid.NewGuid();
            var randomTeacherId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid inputTeacherId = randomTeacherId;
            var exception = new Exception();

            var expectedTeacherContactException =
                new TeacherContactServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId))
                    .ThrowsAsync(exception);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(
                    inputTeacherId,
                    inputContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactServiceException>(() =>
                retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}