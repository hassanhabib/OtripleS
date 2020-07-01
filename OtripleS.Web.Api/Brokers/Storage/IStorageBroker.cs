using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public interface IStorageBroker
    {
        public ValueTask<Student> SelectStudentByIdAsync(Guid studentId);
        public IQueryable<Student> SelectAllStudents();
    }
}
