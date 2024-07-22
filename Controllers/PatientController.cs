using MedwiseBackend.Data;
using MedwiseBackend.Dto;
using MedwiseBackend.Interfaces;
using MedwiseBackend.JwtHelper;
using MedwiseBackend.Models;
using MedwiseBackend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedwiseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [Authorize(Roles = Roles.Patient)]
        [HttpGet("get-doctor")]
        [ProducesResponseType(200, Type = typeof(ICollection<Doctors>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDoctorsOfPatient()
        {
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);
            var result = await _patientRepository.GetAllDoctorsOfPatient(email);
            if (result == null)
            {
                return StatusCode(400, new { success = false, Message = "No Doctors Found" });
            }
            return Ok(result);

        }
        [Authorize(Roles = Roles.Patient)]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdatePatient(UserDto dto)
        {
            var email =  JwtHelper.JwtHelper.GetUserIdFromToken(User);
            bool result=await _patientRepository.UpdateProfile(email, dto);
            if (!result) { return StatusCode(400, new { Success = false, Message = "Updation Failed" });
            }
            return Ok(new {Success = true,Message="Profile Updated"});
        }
    
    }
}
