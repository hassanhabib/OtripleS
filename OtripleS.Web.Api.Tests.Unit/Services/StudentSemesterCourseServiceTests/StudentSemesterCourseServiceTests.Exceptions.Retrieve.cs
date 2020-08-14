// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentSemesterCourseDependencyException
                = new StudentSemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentSemesterCourse> deleteStudentSemesterCourseTask =
                this.studentSemesterCourseService.RetrieveStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseDependencyException>(() =>
                deleteStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }
    }
}
