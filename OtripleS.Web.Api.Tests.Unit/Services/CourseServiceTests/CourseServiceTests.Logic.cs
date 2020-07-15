using OtripleS.Web.Api.Models.Course;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseServiceTests
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldModifyCourseAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dates: randomDate);

            // when
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                .Returns(randomDate);
            // then
        }
    }
}
