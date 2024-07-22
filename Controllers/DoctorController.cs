using MedwiseBackend.Dto;
using MedwiseBackend.Interfaces;
using MedwiseBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MedwiseBackend.JwtHelper;
using MedwiseBackend.Data;
using Microsoft.AspNetCore.Cors;
namespace MedwiseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class DoctorController : Controller
    {
        private readonly IDoctorRespository _doctorRespository;
        private readonly IPatientRepository _patientRepository;
       
        public DoctorController(IDoctorRespository doctorRespository,IPatientRepository patientRepository)
        {
            _doctorRespository = doctorRespository;
            _patientRepository = patientRepository;
            

        }

        [HttpGet("getalldoctor")]
        [ProducesResponseType(200,Type=typeof(ICollection<Doctors>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _doctorRespository.GetAllDoctor();
            if (result == null)
            {
                ModelState.AddModelError("Message", "No Doctors Found ");
                return StatusCode(400, ModelState);
            }
            return Ok(result);
        }

        [HttpGet("getalldoctor/{category}")]
        [ProducesResponseType(200,Type=typeof(ICollection<Doctors>))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetAllDoctorByCategory(string category)
        {
            if (category == null)
            {
                return BadRequest(ModelState);  
            }
            var result=await _doctorRespository.GetAllDoctorByCategory(category);
            if (result == null) {
                ModelState.AddModelError("Message", "No Doctors Found ");
                return StatusCode(400, ModelState);
            }
            return Ok(result);
        }

        [Authorize(Roles = Roles.Doctor)]
        [HttpPost("register-patient")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterPatient([FromBody]RegisterDto dto)
        {
            if (dto == null)
            {
                return BadRequest(ModelState);
            }
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);

            bool result=await _doctorRespository.AddPatient(patientEmail:dto.Email,doctorEmail:email,hospitalId:dto.entityId);
            if (!result)
            {
                ModelState.AddModelError("Message", "Patient could not be registerd");
                return StatusCode(400,ModelState);
            }
            return Ok(new {success=true,Message="Patient Registered"});
        }
        [Authorize(Roles=Roles.Doctor)]
        [HttpGet("get-patients")]
        [ProducesResponseType(200,Type=typeof(ICollection<Patients>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllPatientOfDoctor()
        {
            Console.WriteLine(User.Identity);
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);
            var result = await _doctorRespository.GetAllPatientOfDoctor(email);
            if (result == null)
            {
                ModelState.AddModelError("Message", "No Patients Found");
                return StatusCode(400, ModelState);
            }
            return Ok(result);

        }
        [Authorize(Roles = Roles.Doctor)]
        [HttpPut("update-profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateDoctorProfile(UserDto dto)
        {
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);
            bool result = await _doctorRespository.UpdateProfile(email,dto);
            if (!result)
            {
                return StatusCode(400, new { Success = false, Message = "Failed" });
            }
            return Ok(new { Success = true, Message = "Profile Updated" });
        }
        
        
        
    }
} 
