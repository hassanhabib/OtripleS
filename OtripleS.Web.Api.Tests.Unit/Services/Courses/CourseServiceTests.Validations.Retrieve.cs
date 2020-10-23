// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenCoursesWereEmptyAndLogIt()
        {
            // given
            IQueryable<Course> emptyStorageCourses = new List<Course>().AsQueryable();
            IQueryable<Course> expectedCourses = emptyStorageCourses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourses())
                    .Returns(expectedCourses);

            // when
            IQueryable<Course> actualCourses =
                this.courseService.RetrieveAllCourses();

            // then
            actualCourses.Should().BeEquivalentTo(emptyStorageCourses);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No courses found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourses(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
