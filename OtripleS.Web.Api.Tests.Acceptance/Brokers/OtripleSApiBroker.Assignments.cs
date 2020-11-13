using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Assignments;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string AssignmentsRelativeUrl = "api/assignments";

        public async ValueTask<Assignment> PostAssignmentAsync(Assignment assignment) =>
            await this.apiFactoryClient.PostContentAsync(AssignmentsRelativeUrl, assignment);

        public async ValueTask<Assignment> GetAssignmentByIdAsync(Guid assignmentId) =>
            await this.apiFactoryClient.GetContentAsync<Assignment>($"{AssignmentsRelativeUrl}/{assignmentId}");

        public async ValueTask<Assignment> DeleteAssignmentByIdAsync(Guid assignmentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Assignment>($"{AssignmentsRelativeUrl}/{assignmentId}");

        public async ValueTask<Assignment> PutAssignmentAsync(Assignment assignment) =>
            await this.apiFactoryClient.PutContentAsync(AssignmentsRelativeUrl, assignment);

        public async ValueTask<List<Assignment>> GetAllAssignmentsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Assignment>>($"{AssignmentsRelativeUrl}/");
    }
}