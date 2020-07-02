using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Services;
using System.Linq;
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

        private IQueryable<Student> CreateRandomStudents()
        {
            int randomNumber = new IntRange(min: 2, max: 10).GetValue();
            var filler = new Filler<Student>();

            filler.Setup()
               .OnProperty(student => student.BirthDate).Use(this.dateTimeBroker.GetCurrentDateTime())
               .OnProperty(student => student.CreatedDate).Use(this.dateTimeBroker.GetCurrentDateTime())
               .OnProperty(student => student.UpdatedDate).Use(this.dateTimeBroker.GetCurrentDateTime());

            return filler.Create(randomNumber).AsQueryable();
        }
    }
}
