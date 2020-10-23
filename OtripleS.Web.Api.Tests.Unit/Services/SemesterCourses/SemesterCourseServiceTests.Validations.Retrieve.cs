// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenSemesterCoursesWereEmptyAndLogIt()
        {
            // given
            IQueryable<SemesterCourse> emptyStorageSemesterCourses = new List<SemesterCourse>().AsQueryable();
            IQueryable<SemesterCourse> expectedSemesterCourses = emptyStorageSemesterCourses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSemesterCourses())
                    .Returns(expectedSemesterCourses);

            // when
            IQueryable<SemesterCourse> actualSemesterCourses =
                this.semesterCourseService.RetrieveAllSemesterCourses();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(emptyStorageSemesterCourses);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No semesterSemesterCourses found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSemesterCourses(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
