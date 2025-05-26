
using Microsoft.AspNetCore.Mvc;
using Tutorial5.DTOs;
using Tutorial5.Services;

namespace Tutorial5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IDbService _dbService;
        public HospitalController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionRequestDTO request)
        {
            try
            {
                await _dbService.AddNewPrescriptionAsync(request);
                return Ok("Prescription created successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("patient/{id}")]
        public async Task<IActionResult> GetPatientInfo(int id)
        {
            var patientInfo = await _dbService.GetPatientInfoAsync(id);
            if (patientInfo == null)
            {
                return NotFound($"Patient with id {id} not found.");
            }
            return Ok(patientInfo);
        }
    }
}