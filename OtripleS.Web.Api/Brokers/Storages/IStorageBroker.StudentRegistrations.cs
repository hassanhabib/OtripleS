// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentRegistration> InsertStudentRegistrationAsync(StudentRegistration studentRegistration);
        IQueryable<StudentRegistration> SelectAllStudentRegistrations();
        ValueTask<StudentRegistration> SelectStudentRegistrationByIdAsync(Guid studentId, Guid registrationId);
        ValueTask<StudentRegistration> UpdateStudentRegistrationAsync(StudentRegistration studentRegistration);
        ValueTask<StudentRegistration> DeleteStudentRegistrationAsync(StudentRegistration studentRegistration);
    }
}
