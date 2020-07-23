using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ClassroomServiceTests
{
  public partial class ClassroomServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Throws(sqlException);

            // when 
            Assert.Throws<ClassroomDependencyException>(() =>
                this.classroomService.RetrieveAllClassrooms());

            //then
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedClassroomDependencyException))),
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

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<ClassroomDependencyException>(() =>
                this.classroomService.RetrieveAllClassrooms());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomDependencyException))),
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

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedClassroomServiceException =
                new ClassroomServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Throws(exception);

            // when . then
            Assert.Throws<ClassroomServiceException>(() =>
                this.classroomService.RetrieveAllClassrooms());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomServiceException))),
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
