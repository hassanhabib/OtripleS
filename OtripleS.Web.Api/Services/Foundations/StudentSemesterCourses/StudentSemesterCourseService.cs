﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.StudentSemesterCourses;

namespace OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses
{
    public partial class StudentSemesterCourseService : IStudentSemesterCourseService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentSemesterCourseService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<StudentSemesterCourse> CreateStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse) => TryCatch(async () =>
        {
            ValidateStudentSemesterCourseOnCreate(studentSemesterCourse);

            return await this.storageBroker.InsertStudentSemesterCourseAsync(studentSemesterCourse);
        });

        public IQueryable<StudentSemesterCourse> RetrieveAllStudentSemesterCourses() =>
        TryCatch(() => this.storageBroker.SelectAllStudentSemesterCourses());

        public ValueTask<StudentSemesterCourse> RetrieveStudentSemesterCourseByIdAsync(
          Guid studentId,
          Guid semesterCourse) => TryCatch(async () =>
          {
              ValidateStudentSemesterCourseIdIsNull(studentId, semesterCourse);

              StudentSemesterCourse maybeStudentSemesterCourse =
                   await this.storageBroker.SelectStudentSemesterCourseByIdAsync(studentId, semesterCourse);

              ValidateStorageStudentSemesterCourse(maybeStudentSemesterCourse, studentId, semesterCourse);

              return maybeStudentSemesterCourse;
          });

        public ValueTask<StudentSemesterCourse> ModifyStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse) => TryCatch(async () =>
          {
              ValidateStudentSemesterCourseOnModify(studentSemesterCourse);

              StudentSemesterCourse maybeStudentSemesterCourse =
                  await this.storageBroker.SelectStudentSemesterCourseByIdAsync(
                      studentSemesterCourse.StudentId,
                      studentSemesterCourse.SemesterCourseId);

              ValidateStorageStudentSemesterCourse(
                  maybeStudentSemesterCourse,
                  studentSemesterCourse.StudentId,
                  studentSemesterCourse.SemesterCourseId);

              ValidateAgainstStorageStudentSemesterCourseOnModify(
                  inputStudentSemesterCourse: studentSemesterCourse,
                  storageStudentSemesterCourse: maybeStudentSemesterCourse);

              return await this.storageBroker.UpdateStudentSemesterCourseAsync(studentSemesterCourse);
          });

        public ValueTask<StudentSemesterCourse> RemoveStudentSemesterCourseByIdsAsync(
            Guid semesterCourseId,
            Guid studentId) => TryCatch(async () =>
          {
              ValidateSemesterCourseId(semesterCourseId);
              ValidateStudentId(studentId);

              StudentSemesterCourse studentSemesterCourse =
                  await this.storageBroker.SelectStudentSemesterCourseByIdAsync(semesterCourseId, studentId);

              ValidateStorageStudentSemesterCourse(studentSemesterCourse, semesterCourseId, studentId);

              return await this.storageBroker.DeleteStudentSemesterCourseAsync(studentSemesterCourse);
          });
    }
}
