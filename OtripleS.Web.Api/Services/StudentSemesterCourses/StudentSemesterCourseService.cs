//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OtripleS.Web.Api.Services.StudentSemesterCourses
{
    public partial class StudentSemesterCourseService : IStudentSemesterCourseService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentSemesterCourseService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<StudentSemesterCourse> CreateStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse) =>
            TryCatch(async () =>
            {
                ValidateStudentSemesterCourseOnCreate(studentSemesterCourse);
                return await this.storageBroker.InsertStudentSemesterCourseAsync(studentSemesterCourse);
            });

        public IQueryable<StudentSemesterCourse> RetrieveAllStudentSemesterCourses()
        {
            try
            {
                IQueryable<StudentSemesterCourse> storageStudentSemesterCourses =
                    this.storageBroker.SelectAllStudentSemesterCourses();

                ValidateStorageStudentSemesterCourses(storageStudentSemesterCourses);

                return storageStudentSemesterCourses;
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            
        }
    }
}