//?---------------------------------------------------------------
//?Copyright?(c)?Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//?---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Requests;

namespace OtripleS.Web.Api.Services
{
    public interface IStudentService
    {
        ValueTask<Student> DeleteStudentAsync(Guid studentId);
        ValueTask<Student> ModifyStudentAsync(Guid studentId, StudentUpdateDto updateDto);
        ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId);
    }
}
