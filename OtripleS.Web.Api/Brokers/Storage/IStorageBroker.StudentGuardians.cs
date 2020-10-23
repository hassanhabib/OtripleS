// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentGuardians;

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
