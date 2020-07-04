using System;
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

        public async ValueTask<Student> DeleteStudentAsync(Guid studentId)
        {
            ValidateStudentId(studentId);
            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            ValidateStorageStudent(maybeStudent, studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        }

        public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
        TryCatch(async () =>
        {
            ValidateStudentId(studentId);
            Student storageStudent = await this.storageBroker.SelectStudentByIdAsync(studentId);
            ValidateStorageStudent(storageStudent, studentId);

            return storageStudent;
        });

        public ValueTask<Student> ModifyStudentAsync(Student student)=>
            TryCatch(async () =>
            {
                Student maybeStudent = await this.storageBroker.SelectStudentByIdAsync(student.Id);

                return await this.storageBroker.UpdateStudentAsycn(student);
               
            });    
           
        
    }
}
