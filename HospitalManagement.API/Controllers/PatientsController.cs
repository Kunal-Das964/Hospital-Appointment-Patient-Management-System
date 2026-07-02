using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.RegisterPatientAsync(dto);
            return StatusCode(201, new { PatientId = id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _service.GetAllActivePatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetPatientByIdAsync(id);

            if (patient == null)
                return NotFound(new { message = "Patient not found." });

            return Ok(patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdatePatientAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _service.DeactivatePatientAsync(id);
            return NoContent();
        }
    }
}
