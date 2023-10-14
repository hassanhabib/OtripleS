// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Exams;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamsController : RESTFulController
    {
        private readonly IExamService examService;

        public ExamsController(IExamService examService) =>
            this.examService = examService;

        [HttpPost]
        public async ValueTask<ActionResult<Exam>> PostExamAsync(Exam exam)
        {
            try
            {
                Exam addedExam =
                    await this.examService.AddExamAsync(exam);

                return Created(addedExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is AlreadyExistsExamException)
            {
                return Conflict(examValidationException.GetInnerMessage());
            }
            catch (ExamValidationException examValidationException)
            {
                return BadRequest(examValidationException.GetInnerMessage());
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Exam>> GetAllExams()
        {
            try
            {
                IQueryable storageExam =
                    this.examService.RetrieveAllExams();

                return Ok(storageExam);
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpGet("{examId}")]
        public async ValueTask<ActionResult<Exam>> GetExamAsync(Guid examId)
        {
            try
            {
                Exam storageExam =
                    await this.examService.RetrieveExamByIdAsync(examId);

                return Ok(storageExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is NotFoundExamException)
            {
                return NotFound(examValidationException.GetInnerMessage());
            }
            catch (ExamValidationException examValidationException)
            {
                return BadRequest(examValidationException.GetInnerMessage());
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Exam>> PutExamAsync(Exam exam)
        {
            try
            {
                Exam registeredExam =
                    await this.examService.ModifyExamAsync(exam);

                return Ok(registeredExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is NotFoundExamException)
            {
                return NotFound(examValidationException.GetInnerMessage());
            }
            catch (ExamValidationException examValidationException)
            {
                return BadRequest(examValidationException.GetInnerMessage());
            }
            catch (ExamDependencyException examDependencyException)
                when (examDependencyException.InnerException is LockedExamException)
            {
                return Locked(examDependencyException.GetInnerMessage());
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

        [HttpDelete("{examId}")]
        public async ValueTask<ActionResult<Exam>> DeleteExamAsync(Guid examId)
        {
            try
            {
                Exam storageExam =
                    await this.examService.RemoveExamByIdAsync(examId);

                return Ok(storageExam);
            }
            catch (ExamValidationException examValidationException)
                when (examValidationException.InnerException is NotFoundExamException)
            {
                return NotFound(examValidationException.GetInnerMessage());
            }
            catch (ExamValidationException examValidationException)
            {
                return BadRequest(examValidationException.GetInnerMessage());
            }
            catch (ExamDependencyException examDependencyException)
               when (examDependencyException.InnerException is LockedExamException)
            {
                return Locked(examDependencyException.GetInnerMessage());
            }
            catch (ExamDependencyException examDependencyException)
            {
                return Problem(examDependencyException.Message);
            }
            catch (ExamServiceException examServiceException)
            {
                return Problem(examServiceException.Message);
            }
        }

    }
}
