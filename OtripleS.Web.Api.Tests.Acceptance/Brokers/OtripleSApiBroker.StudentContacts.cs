// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentContacts;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string StudentContactsRelativeUrl = "api/StudentContacts";

        public async ValueTask<StudentContact> PostStudentContactAsync(StudentContact studentContact) =>
            await this.apiFactoryClient.PostContentAsync(StudentContactsRelativeUrl, studentContact);

        public async ValueTask<StudentContact> GetStudentContactAsync(Guid studentId, Guid contactId) =>
            await this.apiFactoryClient.GetContentAsync<StudentContact>($"{StudentContactsRelativeUrl}/students/{studentId}/contacts/{contactId}");

        public async ValueTask<StudentContact> DeleteStudentContactAsync(Guid studentId, Guid contactId) =>
            await this.apiFactoryClient.DeleteContentAsync<StudentContact>($"{StudentContactsRelativeUrl}/students/{studentId}/contacts/{contactId}");

        public async ValueTask<StudentContact> PutStudentContactAsync(StudentContact studentContact) =>
            await this.apiFactoryClient.PutContentAsync(StudentContactsRelativeUrl, studentContact);

        public async ValueTask<List<StudentContact>> GetAllStudentContactsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<StudentContact>>($"{StudentContactsRelativeUrl}/");
    }
}