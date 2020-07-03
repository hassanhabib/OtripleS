using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Services
{
    public interface IStudentService
    {
        ValueTask<Student> DeleteStudentAsync(Guid studentId);
        ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId);
    }
}
