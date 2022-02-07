﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.TeacherAttachments;
using OtripleS.Web.Api.Models.TeacherAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherAttachments
{
    public partial class TeacherAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();
            var expectedTeacherAttachmentDependencyException = new TeacherAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<TeacherAttachment> retrieveTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                retrieveTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var expectedTeacherAttachmentDependencyException =
                new TeacherAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<TeacherAttachment> retrieveTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                retrieveTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedAttachmentException = new LockedTeacherAttachmentException(databaseUpdateConcurrencyException);

            var expectedTeacherAttachmentException =
                new TeacherAttachmentDependencyException(lockedAttachmentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<TeacherAttachment> retrieveTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentDependencyException>(() =>
                retrieveTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttachmentId = Guid.NewGuid();
            Guid someTeacherId = Guid.NewGuid();
            var serviceException = new Exception();
            var expectedTeacherAttachmentException = new TeacherAttachmentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<TeacherAttachment> retrieveTeacherAttachmentTask =
                this.teacherAttachmentService.RetrieveTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId);

            // then
            await Assert.ThrowsAsync<TeacherAttachmentServiceException>(() =>
                retrieveTeacherAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherAttachmentException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherAttachmentByIdAsync(someTeacherId, someAttachmentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
