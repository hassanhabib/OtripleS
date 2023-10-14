// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using OtripleS.Web.Api.Services.Foundations.StudentExams;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentExamsController : RESTFulController
    {
        private readonly IStudentExamService studentExamService;

        public StudentExamsController(IStudentExamService studentExamService) =>
            this.studentExamService = studentExamService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentExam>> PostStudentExamAsync(
            StudentExam studentExam)
        {
            try
            {
                StudentExam addedStudentExam =
                    await this.studentExamService.AddStudentExamAsync(studentExam);

                return Created(addedStudentExam);
            }
            catch (StudentExamValidationException studentExamValidationException)
                when (studentExamValidationException.InnerException is AlreadyExistsStudentExamException)
            {
                return Conflict(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamValidationException studentExamValidationException)
            {
                return BadRequest(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamDependencyException studentExamDependencyException)
            {
                return Problem(studentExamDependencyException.Message);
            }
            catch (StudentExamServiceException studentExamServiceException)
            {
                return Problem(studentExamServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<StudentExam>> GetAllStudentExams()
        {
            try
            {
                IQueryable storageStudentExams =
                    this.studentExamService.RetrieveAllStudentExams();

                return Ok(storageStudentExams);
            }
            catch (StudentExamDependencyException studentExamDependencyException)
            {
                return Problem(studentExamDependencyException.Message);
            }
            catch (StudentExamServiceException studentExamServiceException)
            {
                return Problem(studentExamServiceException.Message);
            }
        }

        [HttpGet("{studentExamId}")]
        public async ValueTask<ActionResult<StudentExam>> GetStudentExamAsync(Guid studentExamId)
        {
            try
            {
                StudentExam storageStudentExam =
                    await this.studentExamService.RetrieveStudentExamByIdAsync(studentExamId);

                return Ok(storageStudentExam);
            }
            catch (StudentExamValidationException studentExamValidationException)
                when (studentExamValidationException.InnerException is NotFoundStudentExamException)
            {
                return NotFound(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamValidationException studentExamValidationException)
            {
                return BadRequest(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamDependencyException studentExamDependencyException)
            {
                return Problem(studentExamDependencyException.Message);
            }
            catch (StudentExamServiceException studentExamServiceException)
            {
                return Problem(studentExamServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<StudentExam>> PutStudentExamAsync(StudentExam studentExam)
        {
            try
            {
                StudentExam registeredStudentExam =
                    await this.studentExamService.ModifyStudentExamAsync(studentExam);

                return Ok(registeredStudentExam);
            }
            catch (StudentExamValidationException studentExamValidationException)
                when (studentExamValidationException.InnerException is NotFoundStudentExamException)
            {
                return NotFound(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamValidationException studentExamValidationException)
            {
                return BadRequest(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamDependencyException studentExamDependencyException)
                when (studentExamDependencyException.InnerException is LockedStudentExamException)
            {
                return Locked(studentExamDependencyException.GetInnerMessage());
            }
            catch (StudentExamDependencyException studentExamDependencyException)
            {
                return Problem(studentExamDependencyException.Message);
            }
            catch (StudentExamServiceException studentExamServiceException)
            {
                return Problem(studentExamServiceException.Message);
            }
        }

        [HttpDelete("{studentExamId}")]
        public async ValueTask<ActionResult<StudentExam>> DeleteStudentExamAsync(Guid studentExamId)
        {
            try
            {
                StudentExam storageStudentExam =
                    await this.studentExamService.RemoveStudentExamByIdAsync(studentExamId);

                return Ok(storageStudentExam);
            }
            catch (StudentExamValidationException studentExamValidationException)
                when (studentExamValidationException.InnerException is NotFoundStudentExamException)
            {
                return NotFound(studentExamValidationException.GetInnerMessage());
            }
            catch (StudentExamValidationException studentExamValidationException)
            {
                return BadRequest(studentExamValidationException.Message);
            }
            catch (StudentExamDependencyException studentExamDependencyException)
                when (studentExamDependencyException.InnerException is LockedStudentExamException)
            {
                return Locked(studentExamDependencyException.GetInnerMessage());
            }
            catch (StudentExamDependencyException studentExamDependencyException)
            {
                return Problem(studentExamDependencyException.Message);
            }
            catch (StudentExamServiceException studentExamServiceException)
            {
                return Problem(studentExamServiceException.Message);
            }
        }

    }
}
