using MedwiseBackend.Dto;
using MedwiseBackend.Models;

namespace MedwiseBackend.Interfaces
{
    public interface IDoctorRespository
    {
        public Task<Doctors> GetDoctorByEmail(string email);
        public Task<ICollection<Doctors>> GetAllDoctor();
        public  Task<ICollection<Doctors>> GetAllDoctorByCategory(string category);
        public Task<bool> AddPatient(string patientEmail, string doctorEmail,int hospitalId);
        public Task<ICollection<Patients>> GetAllPatientOfDoctor(string doctorEmail);
        public  Task<bool> UpdateProfile(string doctorEmail,UserDto dto);
    }
}
