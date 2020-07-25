using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using OtripleS.Web.Api.Services.Classrooms;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClassroomsController : RESTFulController
	{
		private readonly IClassroomService classroomService;

		public ClassroomsController(IClassroomService classroomService) =>
			this.classroomService = classroomService;

		[HttpGet]
		public ActionResult<IQueryable<Classroom>> GetAllClassrooms()
		{
			try
			{
				IQueryable storageClassrooms =
					this.classroomService.RetrieveAllClassrooms();

				return Ok(storageClassrooms);
			}
			catch (ClassroomDependencyException classRoomDependencyException)
			{
				return Problem(classRoomDependencyException.Message);
			}
			catch (ClassroomServiceException classRoomServiceException)
			{
				return Problem(classRoomServiceException.Message);
			}
		}
	}
}
