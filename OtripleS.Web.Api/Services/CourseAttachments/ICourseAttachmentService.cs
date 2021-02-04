using System.Threading.Tasks;
using OtripleS.Web.Api.Models.CourseAttachments;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public interface ICourseAttachmentService
    {
        ValueTask<CourseAttachment> AddCourseAttachmentAsync(CourseAttachment courseAttachment);
    }
}