// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Assignments;

namespace OtripleS.Web.Api.Services.Foundations.Assignments
{
    public partial class AssignmentService : IAssignmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AssignmentService(IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Assignment> CreateAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnCreate(assignment);

            return await this.storageBroker.InsertAssignmentAsync(assignment);
        });

        public IQueryable<Assignment> RetrieveAllAssignments() => 
        TryCatch(() => 
            this.storageBroker.SelectAllAssignments());

        public ValueTask<Assignment> RetrieveAssignmentByIdAsync(Guid assignmentId) =>
        TryCatch(async () =>
        {
            ValidateAssignmentIdIsNull(assignmentId);

            Assignment storageAssignment =
                await this.storageBroker.SelectAssignmentByIdAsync(assignmentId);

            ValidateStorageAssignment(storageAssignment, assignmentId);

            return storageAssignment;
        });

        public ValueTask<Assignment> ModifyAssignmentAsync(Assignment assignment) =>
        TryCatch(async () =>
        {
            ValidateAssignmentOnModify(assignment);

            Assignment maybeAssignment =
                await this.storageBroker.SelectAssignmentByIdAsync(assignment.Id);

            ValidateStorageAssignment(maybeAssignment, assignment.Id);

            ValidateAgainstStorageAssignmentOnModify(
                inputAssignment: assignment,
                storageAssignment: maybeAssignment);

            return await this.storageBroker.UpdateAssignmentAsync(assignment);
        });

        public ValueTask<Assignment> RemoveAssignmentByIdAsync(Guid assignmentId) =>
        TryCatch(async () =>
        {
            ValidateAssignmentIdIsNull(assignmentId);

            Assignment maybeAssignment =
                await this.storageBroker.SelectAssignmentByIdAsync(assignmentId);

            ValidateStorageAssignment(maybeAssignment, assignmentId);

            return await this.storageBroker.DeleteAssignmentAsync(maybeAssignment);
        });
    }
}