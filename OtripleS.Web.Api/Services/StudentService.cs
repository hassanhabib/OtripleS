using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Student> DeleteStudentAsync(Guid studentId)=>
        TryCatch(async()=>
        {
            ValidateStudentId(studentId);

            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            ValidateStorageStudent(maybeStudent, studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        });

        public IQueryable<Student> RetrieveAllStudents()=>
        TryCatch(() => {
            IQueryable<Student> maybeStudents = this.storageBroker.SelectAllStudents();
            ValidateStorageStudents(maybeStudents);

            return maybeStudents;

        });
        
    }
}
