// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Assignments;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : RESTFulController
    {
        private readonly IAssignmentService assignmentService;

        public AssignmentsController(IAssignmentService assignmentService) =>
            this.assignmentService = assignmentService;

        [HttpPost]
        public async ValueTask<ActionResult<Assignment>> PostAssignmentAsync(Assignment assignment)
        {
            try
            {
                Assignment createdAssignment =
                    await this.assignmentService.CreateAssignmentAsync(assignment);

                return Created(createdAssignment);
            }
            catch (AssignmentValidationException assignmentValidationException)
                when (assignmentValidationException.InnerException is AlreadyExistsAssignmentException)
            {
                return Conflict(assignmentValidationException.InnerException);
            }
            catch (AssignmentValidationException assignmentValidationException)
            {
                return BadRequest(assignmentValidationException.InnerException);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
            {
                return InternalServerError(assignmentDependencyException);
            }
            catch (AssignmentServiceException assignmentServiceException)
            {
                return InternalServerError(assignmentServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Assignment>> GetAllAssignments()
        {
            try
            {
                IQueryable storageAssignments =
                    this.assignmentService.RetrieveAllAssignments();

                return Ok(storageAssignments);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
            {
                return Problem(assignmentDependencyException.Message);
            }
            catch (AssignmentServiceException assignmentServiceException)
            {
                return Problem(assignmentServiceException.Message);
            }
        }

        [HttpGet("{assignmentId}")]
        public async ValueTask<ActionResult<Assignment>> GetAssignmentByIdAsync(Guid assignmentId)
        {
            try
            {
                Assignment storageAssignment =
                    await this.assignmentService.RetrieveAssignmentByIdAsync(assignmentId);

                return Ok(storageAssignment);
            }
            catch (AssignmentValidationException assignmentValidationException)
                when (assignmentValidationException.InnerException is NotFoundAssignmentException)
            {
                string innerMessage = GetInnerMessage(assignmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AssignmentValidationException assignmentValidationException)
            {
                string innerMessage = GetInnerMessage(assignmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
            {
                return Problem(assignmentDependencyException.Message);
            }
            catch (AssignmentServiceException assignmentServiceException)
            {
                return Problem(assignmentServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Assignment>> PutAssignmentAsync(Assignment assignment)
        {
            try
            {
                Assignment registeredAssignment =
                    await this.assignmentService.ModifyAssignmentAsync(assignment);

                return Ok(registeredAssignment);
            }
            catch (AssignmentValidationException assignmentValidationException)
                when (assignmentValidationException.InnerException is NotFoundAssignmentException)
            {
                string innerMessage = GetInnerMessage(assignmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AssignmentValidationException assignmentValidationException)
            {
                string innerMessage = GetInnerMessage(assignmentValidationException);

                return BadRequest(innerMessage);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
                when (assignmentDependencyException.InnerException is LockedAssignmentException)
            {
                string innerMessage = GetInnerMessage(assignmentDependencyException);

                return Locked(innerMessage);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
            {
                return Problem(assignmentDependencyException.Message);
            }
            catch (AssignmentServiceException assignmentServiceException)
            {
                return Problem(assignmentServiceException.Message);
            }
        }

        [HttpDelete("{assignmentId}")]
        public async ValueTask<ActionResult<Assignment>> DeleteAssignmentAsync(Guid assignmentId)
        {
            try
            {
                Assignment storageAssignment =
                    await this.assignmentService.RemoveAssignmentByIdAsync(assignmentId);

                return Ok(storageAssignment);
            }
            catch (AssignmentValidationException assignmentValidationException)
                when (assignmentValidationException.InnerException is NotFoundAssignmentException)
            {
                string innerMessage = GetInnerMessage(assignmentValidationException);

                return NotFound(innerMessage);
            }
            catch (AssignmentValidationException assignmentValidationException)
            {
                return BadRequest(assignmentValidationException.Message);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
                when (assignmentDependencyException.InnerException is LockedAssignmentException)
            {
                string innerMessage = GetInnerMessage(assignmentDependencyException);

                return Locked(innerMessage);
            }
            catch (AssignmentDependencyException assignmentDependencyException)
            {
                return Problem(assignmentDependencyException.Message);
            }
            catch (AssignmentServiceException assignmentServiceException)
            {
                return Problem(assignmentServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}