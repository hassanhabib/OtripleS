using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Services
{
    public interface IStudentService
    {
        /// <summary>
        /// Registers a new student
        /// </summary>
        ValueTask<Student> RegisterAsync(Guid id, string firstName, string middleName, string lastName, DateTimeOffset BirthDate, Gender gender);

        ValueTask<Student> DeleteStudentAsync(Guid studentId);
    }
}
