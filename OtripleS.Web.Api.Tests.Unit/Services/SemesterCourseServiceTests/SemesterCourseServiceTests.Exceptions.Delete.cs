using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourseServiceTests
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            SqlException sqlException = GetSqlException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                .ThrowsAsync(sqlException);

            // when
            ValueTask<SemesterCourse> deleteSemesterCourseTask =
                this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() =>
                deleteSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(expectedSemesterCourseDependencyException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}