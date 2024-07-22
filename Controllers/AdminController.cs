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
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-doctors")]
        [ProducesResponseType(200, Type = typeof(ICollection<Doctors>))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> DoctorList()
        {
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);
            var result = _adminRepository.GetDoctorList(email);
            if (result == null)
            {
                return StatusCode(400, new { Success = false, Message = "No Doctors Found" });
            }
            return Ok(new { Success = true, Data = result });

        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("get-patients")]
        [ProducesResponseType(200,Type =typeof(ICollection<Patients>))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> PatientList()
        {
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);
            var result=_adminRepository.GetPatientList(email);
            if(result == null)
            {
                return StatusCode(400, new { Success = false, Message = "No Patients Found" });
            }
            return Ok(new { Success = true, Data = result });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("register-doctor")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> RegisterDoctor([FromBody] UserDto dto)
        {
            var email = JwtHelper.JwtHelper.GetUserIdFromToken(User);
            bool result =await  _adminRepository.RegisterDoctor(email, dto);
            if (result == false) {
                return StatusCode(400, new { Success = false, Message = "Could not register" });
            }
            return Ok(new {Success= true, Message="Register Successfully"});
        }
        [HttpPost("register-admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RegisterAdmin([FromBody]RegisterDto dto)
        {
            if (dto == null) return StatusCode(400, new { Success = false });
            bool result=await _adminRepository.RegisterAdmin(dto);
            if (result == false) return StatusCode(400, new { Success = false, Message = "Failed to Register" });
            return Ok( new { Success = true, Message = "Registered" });
        }
    }
}
