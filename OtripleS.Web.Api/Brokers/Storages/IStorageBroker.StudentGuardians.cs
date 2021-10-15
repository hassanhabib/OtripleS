// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.StudentGuardians;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentGuardian> InsertStudentGuardianAsync(
           StudentGuardian studentGuardian);

        IQueryable<StudentGuardian> SelectAllStudentGuardians();

        ValueTask<StudentGuardian> SelectStudentGuardianByIdAsync(
           Guid studentId,
           Guid guardianId);

        ValueTask<StudentGuardian> UpdateStudentGuardianAsync(
           StudentGuardian studentGuardian);

        ValueTask<StudentGuardian> DeleteStudentGuardianAsync(
           StudentGuardian studentGuardian);
    }
}