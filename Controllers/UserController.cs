using MedwiseBackend.Dto;
using MedwiseBackend.Interfaces;
using MedwiseBackend.Models;
using MedwiseBackend.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedwiseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("generate-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GenerateToken([FromBody] UserDto userdto)
        {
            if (userdto == null)
            {
                return BadRequest(ModelState);
            }
            var token = _userRepository.GenerateToken(userdto);
            if (token == null)
            {
                ModelState.AddModelError("", "No Token Generated");
                return StatusCode(400, ModelState);
            }
            return Ok(token);

        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] UserDto userdto)
        {
            if (userdto == null) {
                return BadRequest(ModelState);
            }
            var result=await _userRepository.AuthenticateAsync(userdto);
            if (result==null) {
                ModelState.AddModelError("Message", "User Not Found");
                return StatusCode(400, ModelState);
            }
            UserDto user = new UserDto()
            {
                Email = userdto.Email,
                Name = result.Name,
                Roles = userdto.Roles

            };
           var token=_userRepository.GenerateToken(user);
            if (token == null) {
                ModelState.AddModelError("Message", "No Token Generated");
                return StatusCode(400, ModelState);
            }
            return Ok(new { success=true, token=token });

        }
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] UserDto userdto)
        {
            if (userdto == null)
            {
                return BadRequest(ModelState);
            }
            var result = await _userRepository.RegisterUser(userdto);
            if (result == null) {
                //  ModelState.AddModelError("Message", "User Already Exist");
                return StatusCode(400, new { Success = false, status = "User Already Exists" });
            }
            return Ok(new { Success=true, result=result });

        }  

    }
}
