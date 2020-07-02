using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Requests;
using OtripleS.Web.Api.Services;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IStudentService studentService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly DateTimeBroker dateTimeBroker;

        public StudentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBroker = new DateTimeBroker();

            this.studentService = new StudentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Student CreateRandomStudent()
        {
            var filler = new Filler<Student>();
            filler.Setup()
                .OnProperty(student => student.BirthDate).Use(this.dateTimeBroker.GetCurrentDateTime())
                .OnProperty(student => student.CreatedDate).Use(this.dateTimeBroker.GetCurrentDateTime())
                .OnProperty(student => student.UpdatedDate).Use(this.dateTimeBroker.GetCurrentDateTime());

            return filler.Create();
        }

        private StudentUpdateDto CreateRandomDto()
        {
            var filler = new Filler<StudentUpdateDto>();
            filler.Setup()
                .OnProperty(dto => dto.BirthDate).Use(this.dateTimeBroker.GetCurrentDateTime().AddYears(-21));
            return filler.Create();
        }

        private Student NewStudentWithUpdatedProperties(Student student, StudentUpdateDto dto)
        {
            var newStudent = new Student
            {
                IdentityNumber = dto.IdentityNumber,
                Gender = GenderHelper.StringToGenderConverter(dto.Gender),
                BirthDate = dto.BirthDate,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Id = student.Id,
                UserId = student.UserId,
                CreatedBy = student.CreatedBy,
                CreatedDate = student.CreatedDate,
                UpdatedBy = student.UpdatedBy,
                UpdatedDate = student.UpdatedDate
            };


            return newStudent;
        }
    }
}