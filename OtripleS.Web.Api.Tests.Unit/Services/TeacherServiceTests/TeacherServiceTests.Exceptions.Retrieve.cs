using Microsoft.EntityFrameworkCore;

using Moq;

using OtripleS.Web.Api.Models.Teachers.Exceptions;

using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherServiceTests
{
    public partial class TeacherServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedTeacherDependencyException =
                new TeacherDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeachers())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<TeacherDependencyException>(() =>
                this.teacherService.RetrieveAllTeachers());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeachers(),
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

            var expectedTeacherDependencyException =
                new TeacherDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeachers())
                    .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<TeacherDependencyException>(() =>
                this.teacherService.RetrieveAllTeachers());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeachers(),
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

            var expectedTeacherServiceException =
                new TeacherServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeachers())
                    .Throws(exception);

            // when . then
            Assert.Throws<TeacherServiceException>(() =>
                this.teacherService.RetrieveAllTeachers());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedTeacherServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeachers(),
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
