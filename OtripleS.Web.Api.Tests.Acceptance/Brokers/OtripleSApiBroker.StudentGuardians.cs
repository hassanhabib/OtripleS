// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentGuardians;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string StudentGuardiansRelativeUrl = "api/studentGuardians";

        public async ValueTask<StudentGuardian> PostStudentGuardianAsync(StudentGuardian studentGuardian) =>
            await this.apiFactoryClient.PostContentAsync(StudentGuardiansRelativeUrl, studentGuardian);

        public async ValueTask<StudentGuardian> GetStudentGuardianAsync(Guid studentId, Guid guardianId) =>
            await this.apiFactoryClient.GetContentAsync<StudentGuardian>($"{StudentGuardiansRelativeUrl}/students/{studentId}/guardians/{guardianId}");

        public async ValueTask<StudentGuardian> DeleteStudentGuardianAsync(Guid studentId, Guid guardianId) =>
            await this.apiFactoryClient.DeleteContentAsync<StudentGuardian>($"{StudentGuardiansRelativeUrl}/students/{studentId}/guardians/{guardianId}");

        public async ValueTask<StudentGuardian> PutStudentGuardianAsync(StudentGuardian studentGuardian) =>
            await this.apiFactoryClient.PutContentAsync(StudentGuardiansRelativeUrl, studentGuardian);

        public async ValueTask<List<StudentGuardian>> GetAllStudentGuardiansAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<StudentGuardian>>($"{StudentGuardiansRelativeUrl}/");
    }
}