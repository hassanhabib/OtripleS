//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentSemesterCoursesWereEmptyAndLogIt()
        {
            // given
            IQueryable<StudentSemesterCourse> emptyStorageStudentSemesterCourses =
                new List<StudentSemesterCourse>().AsQueryable();
            IQueryable<StudentSemesterCourse> expectedStudentSemesterCourses =
                emptyStorageStudentSemesterCourses;

            string expectedMessage = "No studentSemesterSemesterCourses found in storage.";

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentSemesterCourses())
                .Returns(expectedStudentSemesterCourses);

            // when
            IQueryable<StudentSemesterCourse> actualSemesterCourses =
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(emptyStorageStudentSemesterCourses);

            this.loggingBrokerMock.Verify(broker => broker.LogWarning(expectedMessage),
                Times.Once);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}