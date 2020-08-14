// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentSemesterCourses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<StudentSemesterCourse> InsertStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse);

        public IQueryable<StudentSemesterCourse> SelectAllStudentSemesterCourses();

        public ValueTask<StudentSemesterCourse> SelectStudentSemesterCourseByIdAsync(
            Guid studentId, Guid semesterCourseId);

        public ValueTask<StudentSemesterCourse> UpdateStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse);

        public ValueTask<StudentSemesterCourse> DeleteStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse);
    }
}
