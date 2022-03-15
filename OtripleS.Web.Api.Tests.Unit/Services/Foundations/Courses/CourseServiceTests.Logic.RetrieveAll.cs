// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCourses()
        {
            // given
            IQueryable<Course> randomCourses = CreateRandomCourses();
            IQueryable<Course> storageCourses = randomCourses;
            IQueryable<Course> expectedCourses = storageCourses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourses())
                    .Returns(storageCourses);

            // when
            IQueryable<Course> actualCourses =
                this.courseService.RetrieveAllCourses();

            // then
            actualCourses.Should().BeEquivalentTo(expectedCourses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourses(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
