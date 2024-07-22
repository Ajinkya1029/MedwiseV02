using MedwiseBackend.Data;
using MedwiseBackend.Interfaces;
using MedwiseBackend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MedwiseBackend.Dto;
namespace MedwiseBackend.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRespository _doctorRespository;
        private readonly IAdminRepository _adminRepository;
        public UserRepository(ApplicationContext applicationContext,IConfiguration configuration,IPatientRepository patientRepository,IDoctorRespository doctorRespository,IAdminRepository adminRepository)
        {
            _applicationContext = applicationContext;
            _configuration = configuration;
            _patientRepository = patientRepository;
            _doctorRespository = doctorRespository;
            _adminRepository = adminRepository;
        }
        public string GenerateToken(UserDto userdto)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var subject = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("email",userdto.Email),
                    new Claim("roles",userdto.Roles),
                    new Claim("name",userdto.Name)
                 };
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims: subject,
                    expires: DateTime.UtcNow.AddYears(12),
                    signingCredentials: credential
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex) {
                return string.Empty;
            }
        }

       

        public async Task<User> AuthenticateAsync(UserDto userdto)
        {
            var role = userdto.Roles;
            var email = userdto.Email;
            var password = userdto.Password;
            

            if (role == "patient")
            {
                var result = await _applicationContext.Patients.Where(p => p.Email == email && p.Password == password ).FirstOrDefaultAsync();
            
                return result;
            }
            else if (role == "doctor")
            {
                var result = await _applicationContext.Doctors.Where(p => p.Email==email && p.Password == password).FirstOrDefaultAsync();
                return result;
            }
            else if (role == "admin")
            {
                var result =await  _applicationContext.Admin.Where(p => p.Email==email && p.Password == password).FirstOrDefaultAsync();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<User>RegisterUser(UserDto userdto)
        {
            
            var name = userdto.Name;
            var email = userdto.Email;
            var password = userdto.Password;
            
                var result = await _applicationContext.Patients.Where(p => p.Email == email).AnyAsync();
                if (result)
                {
                    return null;
                }
                else
                {
                    Patients patient = new Patients 
                    {
                        Name = name,
                        Email = email,
                        Password = password,
                        Roles="patient"
                        
                       
                    };
                     _applicationContext.Patients.Add(patient);
                    await _applicationContext.SaveChangesAsync();
                    return patient;
                }
            
            return null;
        }

    }
}
