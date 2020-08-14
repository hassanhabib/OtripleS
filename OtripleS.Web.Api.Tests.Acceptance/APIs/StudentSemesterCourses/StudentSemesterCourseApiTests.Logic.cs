// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentSemesterCourses
{
    public partial class StudentSemesterCourseApiTests
    {
        [Fact]
        public async Task ShouldPostStudentSemesterCourseAsync()
        {
            // given
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse();
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;

            StudentSemesterCourse expectedStudentSemesterCourse = CreateExpectedStudentSemesterCourse(inputStudentSemesterCourse);

            // when 
            await this.otripleSApiBroker.PostStudentSemesterCourseAsync(inputStudentSemesterCourse);

            StudentSemesterCourse actualStudentSemesterCourse =
            await this.otripleSApiBroker.GetStudentSemesterCourseByIdAsync
            (
                inputStudentSemesterCourse.StudentId,
                inputStudentSemesterCourse.SemesterCourseId
            );

            // then
            actualStudentSemesterCourse.Should().BeEquivalentTo(expectedStudentSemesterCourse);

            await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(actualStudentSemesterCourse.SemesterCourseId);
        }

        [Fact]
        public async Task ShouldPutStudentSemesterCourseAsync()
        {
            // given
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse();
            await this.otripleSApiBroker.PostStudentSemesterCourseAsync(randomStudentSemesterCourse);
            StudentSemesterCourse modifiedStudentSemesterCourse = UpdateStudentSemesterCourseRandom(randomStudentSemesterCourse);

            // when
            await this.otripleSApiBroker.PutStudentSemesterCourseAsync(modifiedStudentSemesterCourse);

            StudentSemesterCourse expectedSemesterCourse = CreateExpectedStudentSemesterCourse(modifiedStudentSemesterCourse);
            StudentSemesterCourse actualStudentSemesterCourse =
                await this.otripleSApiBroker.GetStudentSemesterCourseByIdAsync
                (
                    randomStudentSemesterCourse.StudentId,
                    randomStudentSemesterCourse.SemesterCourseId
                );

            // then
            actualStudentSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            await this.otripleSApiBroker.DeleteStudentSemesterCourseAsync
                (
                    actualStudentSemesterCourse.StudentId
                //,actualStudentSemesterCourse.SemesterCourseId
                );
        }

        [Fact]
        public async Task ShouldGetAllStudentSemesterCoursesAsync()
        {
            // given
            IEnumerable<StudentSemesterCourse> randomStudentSemesterCourses = CreateRandomStudentSemesterCourses();
            List<StudentSemesterCourse> inputStudentSemesterCourses = randomStudentSemesterCourses.ToList();

            foreach (StudentSemesterCourse studentSemesterCourse in inputStudentSemesterCourses)
            {
                await this.otripleSApiBroker.PostStudentSemesterCourseAsync(studentSemesterCourse);
            }

            List<StudentSemesterCourse> expectedStudentSemesterCourses = inputStudentSemesterCourses.ToList();

            // when
            List<StudentSemesterCourse> actualStudentSemesterCourses =
                await this.otripleSApiBroker.GetAllStudentSemesterCourses();

            // then
            foreach (StudentSemesterCourse expectedStudentSemesterCourse in expectedStudentSemesterCourses)
            {
                StudentSemesterCourse actualStudentSemesterCourse =
                    actualStudentSemesterCourses.Single(studentSemesterCourse =>
                        studentSemesterCourse.StudentId == expectedStudentSemesterCourse.StudentId
                        && studentSemesterCourse.SemesterCourseId == expectedStudentSemesterCourse.SemesterCourseId);

                StudentSemesterCourse expectedReturnedStudentSemesterCourse = CreateExpectedStudentSemesterCourse(expectedStudentSemesterCourse);
                actualStudentSemesterCourse.Should().BeEquivalentTo(expectedReturnedStudentSemesterCourse);
                await this.otripleSApiBroker.DeleteSemesterCourseByIdAsync(actualStudentSemesterCourse.StudentId);
            }
        }
    }
}
