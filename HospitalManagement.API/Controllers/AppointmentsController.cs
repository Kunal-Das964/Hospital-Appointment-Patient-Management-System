using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] BookAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.BookAppointmentAsync(dto);
            return StatusCode(201, new { AppointmentId = id });
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _service.CancelAppointmentAsync(id);
            return NoContent();
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming()
        {
            var result = await _service.GetUpcomingAsync();
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await _service.GetByDoctorAsync(doctorId);
            return Ok(result);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _service.GetConsolidatedReportAsync();
            return Ok(result);
        }

        [HttpGet("report/doctor-counts")]
        public async Task<IActionResult> GetDoctorCounts()
        {
            var result = await _service.GetDoctorCountsAsync();
            return Ok(result);
        }

        [HttpGet("report/revenue")]
        public async Task<IActionResult> GetRevenue()
        {
            var result = await _service.GetRevenueBySpecAsync();
            return Ok(result);
        }

        [HttpGet("report/duplicates")]
        public async Task<IActionResult> GetDuplicates()
        {
            var result = await _service.GetDuplicateBookingsAsync();
            return Ok(result);
        }
    }
}
