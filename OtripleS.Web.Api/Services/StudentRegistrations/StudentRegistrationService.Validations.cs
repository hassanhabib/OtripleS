// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private void ValidateStorageStudentRegistrations(IQueryable<StudentRegistration> storageStudentRegistrations)
        {
            if (storageStudentRegistrations.Count() == 0)
            {
                this.loggingBroker.LogWarning("No studentRegistrations found in storage.");
            }
        }
    }
}
