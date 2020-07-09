// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Services.Teachers
{
    public partial class TeacherService : ITeacherService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public TeacherService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public async ValueTask<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            return await this.storageBroker.InsertTeacherAsync(teacher);
        }

        public ValueTask<Teacher> DeleteTeacherByIdAsync(Guid teacherId) =>
        TryCatch(async () =>
        {
            ValidateTeacherId(teacherId);

            Teacher maybeTeacher =
               await this.storageBroker.SelectTeacherByIdAsync(teacherId);

            ValidateStorageTeacher(maybeTeacher, teacherId);

            return await this.storageBroker.DeleteTeacherAsync(maybeTeacher);
        });

        public IQueryable<Teacher> RetrieveAllTeachers() =>
        TryCatch(() =>
        {
            IQueryable<Teacher> storageTeachers = this.storageBroker.SelectAllTeachers();
            ValidateStorageTeachers(storageTeachers);

            return storageTeachers;
        });

        public ValueTask<Teacher> RetrieveTeacherByIdAsync(Guid teacherId) =>
        TryCatch(async () =>
        {
            ValidateTeacherId(teacherId);

            Teacher storageTeacher =
               await this.storageBroker.SelectTeacherByIdAsync(teacherId);

            ValidateStorageTeacher(storageTeacher, teacherId);

            return storageTeacher;
        });
    }
}
