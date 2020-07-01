using OtripleS.Web.Api.Models.Students;

using System.Linq;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public interface IStorageBroker
    {
        IQueryable<Student> SelectAllStudents();
    }
}
