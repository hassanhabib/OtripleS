// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string StudentsRelativeUrl = "api/students";

        public async ValueTask<Student> PostStudentAsync(Student student) =>
            await this.apiFactoryClient.PostContentAsync(StudentsRelativeUrl, student);

        public async ValueTask<Student> GetStudentByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.GetContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");

        public async ValueTask<Student> DeleteStudentByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");

        public async ValueTask<Student> PutStudentAsync(Student student) =>
            await this.apiFactoryClient.PutContentAsync(StudentsRelativeUrl, student);

        public async ValueTask<List<Student>> GetAllStudentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Student>>($"{StudentsRelativeUrl}/");
    }
}
