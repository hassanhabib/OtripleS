using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Student> DeleteStudentAsync(Student student);
    }
}
