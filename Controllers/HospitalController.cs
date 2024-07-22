    using MedwiseBackend.Interfaces;
using MedwiseBackend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedwiseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;
        public HospitalController(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        [HttpGet("getallhospital")]
        [ProducesResponseType(200,Type=typeof(ICollection<Hospitals>))]
        [ProducesResponseType(400)]
        public async  Task<IActionResult> GetAllHospitals()
        {
            var result = await _hospitalRepository.GetAllHospitalAsync();
            if (result == null)
            {
           
                return StatusCode(400, new {Success=false,Message="No Hospitals Found"});
            }
            return Ok(new {Success=true,Data=result});
        }
        [HttpGet("getallhospital/{city}")]
        [ProducesResponseType(200,Type=typeof(ICollection<Hospitals>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllHospitalsByCity(string city) {
        var result=await _hospitalRepository.GetAllHospitalByCityAsync(city);
            if (result == null)
            {
                return StatusCode(400, new { Success = false, Message = "No Hospitals Found" });
            }
            return Ok(new { Success = true, Data = result });
        }
    }
}
