// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;
using OtripleS.Web.Api.Tests.Acceptance.Models.Courses;
using OtripleS.Web.Api.Tests.Acceptance.Models.SemesterCourses;
using OtripleS.Web.Api.Tests.Acceptance.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.SemesterCourses
{
    public partial class SemesterCoursesApiTests
    {
        [Fact]
        public async Task ShouldPostSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = await CreateRandomSemesterCourseAsync();
            SemesterCourse inputSemesterCourse = randomSemesterCourse;

            SemesterCourse expectedSemesterCourse =
                CreateExpectedSemesterCourse(inputSemesterCourse);

            // when 
            await this.otripleSApiBroker.PostSemesterCourseAsync(inputSemesterCourse);

            SemesterCourse actualSemesterCourse =
                await this.otripleSApiBroker.GetSemesterCourseByIdAsync(inputSemesterCourse.Id);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            await DeleteSemesterCourseAsync(actualSemesterCourse);
        }

        [Fact]
        public async Task ShouldPutSemesterCourseAsync()
        {
            // given
            SemesterCourse randomSemesterCourse = await CreateRandomSemesterCourseAsync();
            await this.otripleSApiBroker.PostSemesterCourseAsync(randomSemesterCourse);
            SemesterCourse modifiedSemesterCourse = UpdateSemesterCourseRandom(randomSemesterCourse);

            // when
            await this.otripleSApiBroker.PutSemesterCourseAsync(modifiedSemesterCourse);

            SemesterCourse expectedSemesterCourse =
               CreateExpectedSemesterCourse(modifiedSemesterCourse);

            SemesterCourse actualSemesterCourse =
                await this.otripleSApiBroker.GetSemesterCourseByIdAsync(randomSemesterCourse.Id);
            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            await DeleteSemesterCourseAsync(actualSemesterCourse);
        }

        [Fact]
        public async Task ShouldGetAllSemesterCoursesAsync()
        {
            // given
            Teacher randomTeacher = await PostTeacherAsync();
            Classroom randomClassroom = await PostClassRoomAsync();
            Course randomCourse = await PostCourseAsync();
            List<SemesterCourse> randomSemesterCourses = new List<SemesterCourse>();

            for (int i = 0; i <= GetRandomNumber(); i++)
            {
                randomSemesterCourses.Add(
                    await CreateRandomSemesterCourseAsync(randomTeacher, randomClassroom, randomCourse));
            }

            List<SemesterCourse> inputSemesterCourses = randomSemesterCourses.ToList();
            List<SemesterCourse> expectedSemesterCourses = inputSemesterCourses.ToList();

            // when
            List<SemesterCourse> actualSemesterCourses =
                await this.otripleSApiBroker.GetAllSemesterCoursesAsync();

            // then
            foreach (SemesterCourse expectedSemesterCourse in expectedSemesterCourses)
            {
                SemesterCourse actualSemesterCourse =
                    actualSemesterCourses.Single(semesterCourse =>
                        semesterCourse.Id == expectedSemesterCourse.Id);

                SemesterCourse expectedReturnedCourse = CreateExpectedSemesterCourse(expectedSemesterCourse);
                actualSemesterCourse.Should().BeEquivalentTo(expectedReturnedCourse);
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(actualSemesterCourse.Id);
            }
            await this.otripleSApiBroker.DeleteCourseByIdAsync(randomCourse.Id);
            await this.otripleSApiBroker.DeleteClassroomByIdAsync(randomClassroom.Id);
            await this.otripleSApiBroker.DeleteTeacherByIdAsync(randomTeacher.Id);
        }
    }
}
