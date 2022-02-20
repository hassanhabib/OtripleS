// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedClassroomStorageException =
                new FailedClassroomStorageException(sqlException);

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(failedClassroomStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Throws(sqlException);

            // when
            Action retrieveAllClassroomsAction = () => this.classroomService.RetrieveAllClassrooms();

            // then
            Assert.Throws<ClassroomDependencyException>(retrieveAllClassroomsAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClassrooms(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClassroomDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedClassroomServiceException =
                new FailedClassroomServiceException(serviceException);

            var expectedClassroomServiceException =
                new ClassroomServiceException(failedClassroomServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Throws(serviceException);

            // when
            Action retrieveAllClassroomsAction = () =>
                this.classroomService.RetrieveAllClassrooms();

            // then
            Assert.Throws<ClassroomServiceException>(retrieveAllClassroomsAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClassrooms(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}