//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentSemesterCourses
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

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentSemesterCourses())
                .Throws(sqlException);

            // when
            Action retrieveAllStudentSemesterCourseAction = () =>
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();

            // then
            Assert.Throws<StudentSemesterCourseDependencyException>(
                retrieveAllStudentSemesterCourseAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogCritical(It.Is(SameExceptionAs(
                        expectedStudentSemesterCourseDependencyException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenExceptionOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var expectedStudentSemesterCourseServiceException =
                new StudentSemesterCourseServiceException(serviceException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentSemesterCourses())
                .Throws(serviceException);

            // when
            Action retrieveAllStudentSemesterCourseAction = () =>
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();

            // then
            Assert.Throws<StudentSemesterCourseServiceException>(
                retrieveAllStudentSemesterCourseAction);

            this.loggingBrokerMock.Verify(broker =>
                    broker.LogError(It.Is(SameExceptionAs(
                        expectedStudentSemesterCourseServiceException))),
                            Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}