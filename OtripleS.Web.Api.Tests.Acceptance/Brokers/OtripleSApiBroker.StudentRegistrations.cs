// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string StudentRegistrationsRelativeUrl = "api/studentRegistrations";

        public async ValueTask<StudentRegistration> PostStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            return await this.apiFactoryClient.PostContentAsync(
                StudentRegistrationsRelativeUrl, 
                studentRegistration);
        }

        public async ValueTask<StudentRegistration> GetStudentRegistrationByIdsAsync(
            Guid studentId,
            Guid registrationId)
        {
            return await this.apiFactoryClient.GetContentAsync<StudentRegistration>(
                $"{StudentRegistrationsRelativeUrl}/students/{studentId}/registrations/{registrationId}");
        }

        public async ValueTask<StudentRegistration> DeleteStudentRegistrationAsync(
            Guid studentId,
            Guid registrationId)
        {
            return await this.apiFactoryClient.DeleteContentAsync<StudentRegistration>(
                $"{StudentRegistrationsRelativeUrl}/students/{studentId}/registrations/{registrationId}");
        }

        public async ValueTask<StudentRegistration> PutStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            return await this.apiFactoryClient.PutContentAsync(
                StudentRegistrationsRelativeUrl, 
                studentRegistration);
        }

        public async ValueTask<List<StudentRegistration>> GetAllStudentRegistrationsAsync()
        {
            return await this.apiFactoryClient.GetContentAsync<List<StudentRegistration>>(
                $"{StudentRegistrationsRelativeUrl}/");
        }
    }
}