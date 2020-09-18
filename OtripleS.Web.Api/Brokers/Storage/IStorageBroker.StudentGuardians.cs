// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentGuardians;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<StudentGuardian> InsertStudentGuardianAsync(
            StudentGuardian studentGuardian);

        public IQueryable<StudentGuardian> SelectAllStudentGuardians();

        public ValueTask<StudentGuardian> SelectStudentGuardianByIdAsync(
            Guid studentId,
            Guid GuardianId);

        public ValueTask<StudentGuardian> UpdateStudentGuardianAsync(
            StudentGuardian studentGuardian);

        public ValueTask<StudentGuardian> DeleteStudentGuardianAsync(
            StudentGuardian studentGuardian);
    }
}
