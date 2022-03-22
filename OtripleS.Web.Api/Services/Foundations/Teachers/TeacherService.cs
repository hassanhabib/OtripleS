// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Services.Foundations.Teachers
{
    public partial class TeacherService : ITeacherService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public TeacherService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Teacher> AddTeacherAsync(Teacher teacher) =>
        TryCatch(async () =>
        {
            ValidateTeacherOnCreate(teacher);

            return await this.storageBroker.InsertTeacherAsync(teacher);
        });

        public IQueryable<Teacher> RetrieveAllTeachers() =>
        TryCatch(() => this.storageBroker.SelectAllTeachers());

        public ValueTask<Teacher> RetrieveTeacherByIdAsync(Guid teacherId) =>
        TryCatch(async () =>
        {
            ValidateTeacherId(teacherId);

            Teacher storageTeacher =
               await this.storageBroker.SelectTeacherByIdAsync(teacherId);

            ValidateStorageTeacher(storageTeacher, teacherId);

            return storageTeacher;
        });

        public ValueTask<Teacher> ModifyTeacherAsync(Teacher teacher) =>
        TryCatch(async () =>
         {
             ValidateTeacherOnModify(teacher);
             Teacher maybeTeacher = await this.storageBroker.SelectTeacherByIdAsync(teacher.Id);
             ValidateStorageTeacher(maybeTeacher, teacher.Id);
             ValidateAgainstStorageTeacherOnModify(inputTeacher: teacher, storageTeacher: maybeTeacher);

             return await this.storageBroker.UpdateTeacherAsync(teacher);
         });

        public ValueTask<Teacher> RemoveTeacherByIdAsync(Guid teacherId) =>
        TryCatch(async () =>
        {
            ValidateTeacherId(teacherId);

            Teacher maybeTeacher =
               await this.storageBroker.SelectTeacherByIdAsync(teacherId);

            ValidateStorageTeacher(maybeTeacher, teacherId);

            return await this.storageBroker.DeleteTeacherAsync(maybeTeacher);
        });
    }
}
