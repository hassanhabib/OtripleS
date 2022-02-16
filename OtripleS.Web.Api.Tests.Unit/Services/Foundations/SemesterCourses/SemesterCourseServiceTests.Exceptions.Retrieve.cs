﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSemesterCourses())
                    .Throws(sqlException);

            // when
            Action retrieveAllSemesterCoursesAction = () =>
                this.semesterCourseService.RetrieveAllSemesterCourses();

            // then
            Assert.Throws<SemesterCourseDependencyException>(
                retrieveAllSemesterCoursesAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedSemesterCourseDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSemesterCourses(),
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

            var expectedSemesterCourseServiceException =
                new SemesterCourseServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSemesterCourses())
                    .Throws(serviceException);

            // when
            Action retrieveAllSemesterCoursesAction = () =>
                this.semesterCourseService.RetrieveAllSemesterCourses();

            // then
            Assert.Throws<SemesterCourseServiceException>(
                retrieveAllSemesterCoursesAction);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSemesterCourseServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSemesterCourses(),
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
