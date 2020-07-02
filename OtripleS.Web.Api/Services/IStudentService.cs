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
    }
}
