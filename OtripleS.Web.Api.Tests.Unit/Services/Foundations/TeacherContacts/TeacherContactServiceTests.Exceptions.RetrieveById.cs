// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(someTeacherId,someContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactDependencyException>(() =>
                retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherContactDependencyException =
                new TeacherContactDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(someTeacherId, someContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactDependencyException>(
                () => retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someContactId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedTeacherContactException =
                new FailedTeacherContactServiceException(serviceException);

            var expectedTeacherContactException =
                new TeacherContactServiceException(failedTeacherContactException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TeacherContact> retrieveTeacherContactTask =
                this.teacherContactService.RetrieveTeacherContactByIdAsync(someTeacherId, someContactId);

            // then
            await Assert.ThrowsAsync<TeacherContactServiceException>(() =>
                retrieveTeacherContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}