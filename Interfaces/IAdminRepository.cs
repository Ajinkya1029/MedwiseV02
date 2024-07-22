using MedwiseBackend.Dto;
using MedwiseBackend.Models;

namespace MedwiseBackend.Interfaces
{
    public interface IAdminRepository
    {
        public  Task<Admin> GetAdminByEmail(string adminEmail);
        public  Task<ICollection<Doctors>> GetDoctorList(string adminEmail);
        public Task<ICollection<Patients>> GetPatientList(string adminEmail);
        public Task<bool> RegisterDoctor(string adminEmail, UserDto dto);
        public Task<bool>RegisterAdmin(RegisterDto dto);
    }
}
