// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public void ShouldRetireveAllSemesterCourses()
        {
            // given
            IQueryable<SemesterCourse> randomSemesterCourses =
                CreateRandomSemesterCourses();

            IQueryable<SemesterCourse> storageSemesterCourses = randomSemesterCourses;
            IQueryable<SemesterCourse> expectedSemesterCourses = storageSemesterCourses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSemesterCourses())
                    .Returns(storageSemesterCourses);

            // when
            IQueryable<SemesterCourse> actualSemesterCourses =
                this.semesterCourseService.RetrieveAllSemesterCourses();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(expectedSemesterCourses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSemesterCourses(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
