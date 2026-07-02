using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;
        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors([FromQuery] string? specialization, [FromQuery] bool? isAvailable)
        {
            var doctors = await _service.GetDoctorsAsync(specialization, isAvailable);
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _service.GetDoctorByIdAsync(id);

            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });

            return Ok(doctor);
        }
    }
}
