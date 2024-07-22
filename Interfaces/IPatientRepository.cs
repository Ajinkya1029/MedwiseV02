using MedwiseBackend.Dto;
using MedwiseBackend.Models;

namespace MedwiseBackend.Interfaces
{
    public interface IPatientRepository
    {
        public Task<Patients> GetPatientByEmail(string email);
        public  Task<ICollection<Doctors>> GetAllDoctorsOfPatient(string patientEmail);
        public  Task<bool> UpdateProfile(string patientEmail, UserDto dto);
    }
}
