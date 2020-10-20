using OtripleS.Web.Api.Models.StudentContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
   public partial class OtripleSApiBroker
    {
        private const string StudentContactRelativeUrl = "api/studentContacts";
        public async ValueTask<StudentContact> PostStudentContactAsync(StudentContact studentContact) =>
            await this.apiFactoryClient.PostContentAsync(StudentContactRelativeUrl, studentContact);

        public async ValueTask<StudentContact> GetStudentContactByIdAsync(Guid studentId, Guid contactId) =>
          await this.apiFactoryClient.GetContentAsync<StudentContact>(
              $"{StudentContactRelativeUrl}/students/{studentId}/contacts/{contactId}");

        public async ValueTask<StudentContact> DeleteStudentContactByIdAsync(Guid studentId, Guid contactId) =>
           await this.apiFactoryClient.DeleteContentAsync<StudentContact>(
               $"{StudentContactRelativeUrl}/students/{studentId}/contacts/{contactId}");
    }
}
