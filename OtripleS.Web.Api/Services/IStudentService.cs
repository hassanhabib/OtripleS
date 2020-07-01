using OtripleS.Web.Api.Models.Students;
using System;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services
{
	public interface IStudentService
	{
		ValueTask<Student> DeleteStudentAsync(Guid studentId);
	}
}
