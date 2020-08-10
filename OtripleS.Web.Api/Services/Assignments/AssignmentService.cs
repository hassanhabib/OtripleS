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
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Services.Assignments
{
    public partial class AssignmentService : IAssignmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AssignmentService(IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Assignment> CreateAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnCreate(assignment);

            return await this.storageBroker.InsertAssignmentAsync(assignment);
        });

        public ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnModify(assignment);
            Assignment maybeAssignment = await this.storageBroker.SelectAssignmentByIdAsync(assignment.Id);

            ValidateStorageAssignment(maybeAssignment, assignment.Id);

            ValidateAgainstStorageAssignmentOnModify(
                inputAssignment: assignment,
                storageAssignment: maybeAssignment);

            return await this.storageBroker.UpdateAssignmentAsync(assignment);
        });

        public IQueryable<Assignment> RetrieveAllAssignments() =>
        TryCatch(() =>
        {
            IQueryable<Assignment> storageAssignments = this.storageBroker.SelectAllAssignments();
            ValidateStorageAssignments(storageAssignments);

            return storageAssignments;
        });

        public ValueTask<Assignment> RetrieveAssignmentById(Guid guid) =>
        TryCatch(async () =>
        {
            ValidateAssignmentIdIsNull(guid);
            Assignment storageAssignment = await this.storageBroker.SelectAssignmentByIdAsync(guid);
            ValidateStorageAssignment(storageAssignment, guid);

            return storageAssignment;
        });

        public ValueTask<Assignment> DeleteAssignmentAsync(Guid assignmentId) =>
        TryCatch(async () =>
        {
            Assignment maybeAssignment =
                await this.storageBroker.SelectAssignmentByIdAsync(assignmentId);

            ValidateStorageAssignment(maybeAssignment, assignmentId);

            return await this.storageBroker.DeleteAssignmentAsync(maybeAssignment);
        });
    }
}