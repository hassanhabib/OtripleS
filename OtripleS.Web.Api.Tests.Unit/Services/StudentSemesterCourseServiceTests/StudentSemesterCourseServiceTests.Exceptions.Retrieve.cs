using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
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

            //when
            IQueryable<StudentSemesterCourse> studentSemesterCoursesTask =
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();
            
            // then
            Assert.Throws<StudentSemesterCourseDependencyException>(() =>
                studentSemesterCoursesTask);

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
    }
}