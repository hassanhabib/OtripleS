using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Requests;
using OtripleS.Web.Api.Services;
using OtripleS.Web.Api.Utils;

namespace OtripleS.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpDelete("{studentId}")]
        public async ValueTask<ActionResult<Student>> DeleteStudentAsync(Guid studentId)
        {
            Student storageStudent =
                await this.studentService.DeleteStudentAsync(studentId);

            return Ok(storageStudent);
        }

        [HttpPut("{studentId}")]
        public async ValueTask<ActionResult<Student>> UpdateStudentAsync(Guid studentId,
            [FromBody] StudentUpdateDto dto)
        {
            var student = await this.studentService.ModifyStudentAsync(studentId, dto);

            if (student.HasValue()) return Ok(student);

            return BadRequest("update student failed");
        }
    }
}