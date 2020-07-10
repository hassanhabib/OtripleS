using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string TeachersRelativeUrl = "api/teachers";

        public async ValueTask<Teacher> PostTeacherAsync(Teacher student) =>
            await this.apiFactoryClient.PostContentAsync(TeachersRelativeUrl, student);

        public async ValueTask<Teacher> GetTeacherByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.GetContentAsync<Teacher>($"{TeachersRelativeUrl}/{studentId}");

        public async ValueTask<Teacher> DeleteTeacherByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Teacher>($"{TeachersRelativeUrl}/{studentId}");

        public async ValueTask<Teacher> PutTeacherAsync(Teacher student) =>
            await this.apiFactoryClient.PutContentAsync(TeachersRelativeUrl, student);

        public async ValueTask<List<Teacher>> GetAllTeachers() =>
            await this.apiFactoryClient.GetContentAsync<List<Teacher>>($"{TeachersRelativeUrl}/");

    }
}
