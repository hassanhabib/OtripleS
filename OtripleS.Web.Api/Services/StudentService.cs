using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models;
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

        public async ValueTask<Student> DeleteStudentAsync(Guid studentId)
        {
            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        }

        public async ValueTask<Student> RegisterAsync(
            Guid id,
            string firstName,
            string middleName,
            string lastName,
            DateTimeOffset BirthDate,
            Gender gender
        )
        {
            Student student = await storageBroker.AddStudentAsync(new Student(id, firstName, middleName, lastName, BirthDate, gender))
                .ConfigureAwait(false);

            return student;
        }
    }
}
