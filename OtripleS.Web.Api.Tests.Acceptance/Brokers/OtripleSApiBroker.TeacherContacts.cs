// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.TeacherContacts;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string TeacherContactsRelativeUrl = "api/teachercontacts";

        public async ValueTask<TeacherContact> PostTeacherContactAsync(TeacherContact teacher) =>
            await this.apiFactoryClient.PostContentAsync(TeacherContactsRelativeUrl, teacher);

        public async ValueTask<TeacherContact> GetTeacherContactByIdAsync(Guid teacherId, Guid contactId) =>
            await this.apiFactoryClient.GetContentAsync<TeacherContact>(
                $"{TeacherContactsRelativeUrl}/teachers/{teacherId}/contacts/{contactId}");

        public async ValueTask<TeacherContact> DeleteTeacherContactByIdAsync(Guid teacherId, Guid contactId) =>
            await this.apiFactoryClient.DeleteContentAsync<TeacherContact>(
                $"{TeacherContactsRelativeUrl}/teachers/{teacherId}/contacts/{contactId}");

        public async ValueTask<List<TeacherContact>> GetAllTeacherContactsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<TeacherContact>>($"{TeacherContactsRelativeUrl}/");
    }
}
