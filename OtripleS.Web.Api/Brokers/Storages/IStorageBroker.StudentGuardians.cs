// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentGuardian> InsertStudentGuardianAsync(
           StudentGuardian studentGuardian);

        IQueryable<StudentGuardian> SelectAllStudentGuardians();

        ValueTask<StudentGuardian> SelectStudentGuardianByIdAsync(
           Guid studentId,
           Guid GuardianId);

        ValueTask<StudentGuardian> UpdateStudentGuardianAsync(
           StudentGuardian studentGuardian);

        ValueTask<StudentGuardian> DeleteStudentGuardianAsync(
           StudentGuardian studentGuardian);
    }
}