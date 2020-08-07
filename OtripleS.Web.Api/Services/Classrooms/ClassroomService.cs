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
using OtripleS.Web.Api.Models.Classrooms;

namespace OtripleS.Web.Api.Services.Classrooms
{
    public partial class ClassroomService : IClassroomService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public ClassroomService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Classroom> CreateClassroomAsync(Classroom classroom) =>
        TryCatch(async () =>
        {
            ValidateClassroomOnCreate(classroom);

            return await this.storageBroker.InsertClassroomAsync(classroom);
        });

        public ValueTask<Classroom> DeleteClassroomAsync(Guid classroomId) =>
        TryCatch(async () =>
        {
            ValidateClassroomIdIsNull(classroomId);

            Classroom maybeClassroom =
                await this.storageBroker.SelectClassroomByIdAsync(classroomId);

            ValidateStorageClassroom(maybeClassroom, classroomId);

            return await this.storageBroker.DeleteClassroomAsync(maybeClassroom);
        });

        public ValueTask<Classroom> ModifyClassroomAsync(Classroom classroom) =>
        TryCatch(async () =>
        {
            ValidateClassroomOnModify(classroom);

            Classroom maybeClassroom =
                await this.storageBroker.SelectClassroomByIdAsync(classroom.Id);

            ValidateStorageClassroom(maybeClassroom, classroom.Id);

            ValidateAgainstStorageClassroomOnModify(
                inputClassroom: classroom,
                storageClassroom: maybeClassroom);

            return await this.storageBroker.UpdateClassroomAsync(classroom);
        });

        public IQueryable<Classroom> RetrieveAllClassrooms() =>
        TryCatch(() =>
        {
            IQueryable<Classroom> storageClassrooms = this.storageBroker.SelectAllClassrooms();
            ValidateStorageClassrooms(storageClassrooms);

            return storageClassrooms;
        });

        public ValueTask<Classroom> RetrieveClassroomById(Guid classroomId) =>
        TryCatch(async () =>
        {
            ValidateClassroomIdIsNull(classroomId);

            Classroom storageClassroom =
                await this.storageBroker.SelectClassroomByIdAsync(classroomId);

            ValidateStorageClassroom(storageClassroom, classroomId);

            return storageClassroom;
        });
    }
}