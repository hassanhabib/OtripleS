using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentGuardians;

namespace OtripleS.Web.Api.Services.StudentGuardians
{
	public interface IStudentGuardianService
	{
		ValueTask<StudentGuardian> ModifyStudentAsync(StudentGuardian studentGuardian);
	}
}
