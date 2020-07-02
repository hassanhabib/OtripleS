using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Requests;
using OtripleS.Web.Api.Utils;

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

        public ValueTask<Student> ModifyStudentAsync(Guid studentId, StudentUpdateDto updateDto)
        {
            return TryCatch(async () =>
            {
                var student =
                    await this.storageBroker.SelectStudentByIdAsync(studentId);

                // MapChangesToStudent(updateDto, student);
                ValidateStudent(student);

                return await this.storageBroker.UpdateStudentAsync(student);
            });
        }

        private static void MapChangesToStudent(StudentUpdateDto updateDto, Student student)
        {
            student.IdentityNumber = updateDto.IdentityNumber.HasValue()
                ? updateDto.IdentityNumber
                : student.IdentityNumber;
            student.FirstName = updateDto.FirstName.HasValue()
                ? updateDto.FirstName
                : student.FirstName;
            student.MiddleName = updateDto.MiddleName.HasValue()
                ? updateDto.MiddleName
                : student.MiddleName;
            student.LastName = updateDto.LastName.HasValue()
                ? updateDto.LastName
                : student.LastName;
            student.BirthDate = updateDto.BirthDate.HasValue()
                ? updateDto.BirthDate
                : student.BirthDate;
            student.Gender = updateDto.Gender.HasValue()
                ? updateDto.Gender
                : student.Gender;
        }
    }
}