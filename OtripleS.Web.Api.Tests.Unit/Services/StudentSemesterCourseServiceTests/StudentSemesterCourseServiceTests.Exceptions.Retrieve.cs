using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourseServiceTests
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedStudentSemesterCourseDependencyException =
                new StudentSemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAllStudentSemesterCourses())
                .Throws(sqlException);

            //when. then
            Assert.Throws<StudentSemesterCourseDependencyException>(() =>
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(expectedStudentSemesterCourseDependencyException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAllStudentSemesterCourses())
                .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<StudentSemesterCourseDependencyException>(() =>
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseDependencyException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var exception = new Exception();

            var expectedStudentSemesterCourseServiceException =
                new StudentSemesterCourseServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectAllStudentSemesterCourses())
                .Throws(exception);

            // when . then
            Assert.Throws<StudentSemesterCourseServiceException>(() =>
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseServiceException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}