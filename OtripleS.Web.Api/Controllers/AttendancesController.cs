﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using OtripleS.Web.Api.Services.Foundations.Attendances;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : RESTFulController
    {
        private readonly IAttendanceService attendanceService;

        public AttendancesController(IAttendanceService attendanceService) =>
            this.attendanceService = attendanceService;

        [HttpPost]
        public async ValueTask<ActionResult<Attendance>> PostAttendanceAsync(Attendance attendance)
        {
            try
            {
                Attendance createdAttendance =
                    await this.attendanceService.CreateAttendanceAsync(attendance);

                return Created(createdAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is AlreadyExistsAttendanceException)
            {
                return Conflict(attendanceValidationException.InnerException);
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.InnerException);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return InternalServerError(attendanceDependencyException);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return InternalServerError(attendanceServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Attendance>> GetAllAttendances()
        {
            try
            {
                IQueryable storageAttendance =
                    this.attendanceService.RetrieveAllAttendances();

                return Ok(storageAttendance);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        [HttpGet("{attendanceId}")]
        public async ValueTask<ActionResult<Attendance>> GetAttendanceByIdAsync(Guid attendanceId)
        {
            try
            {
                Attendance attendance =
                    await this.attendanceService.RetrieveAttendanceByIdAsync(attendanceId);

                return Ok(attendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                return NotFound(attendanceValidationException.GetInnerMessage());
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.GetInnerMessage());
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Attendance>> PutAttendanceAsync(Attendance attendance)
        {
            try
            {
                Attendance registeredAttendance =
                    await this.attendanceService.ModifyAttendanceAsync(attendance);

                return Ok(registeredAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                return NotFound(attendanceValidationException.GetInnerMessage());
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.GetInnerMessage());
            }
            catch (AttendanceDependencyException attendanceDependencyException)
                when (attendanceDependencyException.InnerException is LockedAttendanceException)
            {
                return Locked(attendanceDependencyException.GetInnerMessage());
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }

        [HttpDelete("{attendanceId}")]
        public async ValueTask<ActionResult<Attendance>> DeleteAttendanceAsync(Guid attendanceId)
        {
            try
            {
                Attendance storageAttendance =
                    await this.attendanceService.RemoveAttendanceByIdAsync(attendanceId);

                return Ok(storageAttendance);
            }
            catch (AttendanceValidationException attendanceValidationException)
                when (attendanceValidationException.InnerException is NotFoundAttendanceException)
            {
                return NotFound(attendanceValidationException.GetInnerMessage());
            }
            catch (AttendanceValidationException attendanceValidationException)
            {
                return BadRequest(attendanceValidationException.Message);
            }
            catch (AttendanceDependencyException attendanceDependencyException)
                when (attendanceDependencyException.InnerException is LockedAttendanceException)
            {
                return Locked(attendanceDependencyException.GetInnerMessage());
            }
            catch (AttendanceDependencyException attendanceDependencyException)
            {
                return Problem(attendanceDependencyException.Message);
            }
            catch (AttendanceServiceException attendanceServiceException)
            {
                return Problem(attendanceServiceException.Message);
            }
        }


    }
}