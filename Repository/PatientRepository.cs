using Microsoft.EntityFrameworkCore;
using MedwiseBackend.Interfaces;
using MedwiseBackend.Data;
using System.Threading.Tasks;
using MedwiseBackend.Models;
using MedwiseBackend.Dto;

namespace MedwiseBackend.Repository
{
    public class PatientRepository:IPatientRepository
    {
        private readonly ApplicationContext _applicationContext;
            public PatientRepository(ApplicationContext applicationContext)
            {
            _applicationContext = applicationContext;
            }
        public async Task<Patients> GetPatientByEmail(string email)
        {
            var patient=await _applicationContext.Patients.Where(p=>p.Email==email).FirstOrDefaultAsync();
            return patient;
        }
        public async Task<ICollection<Doctors>>GetAllDoctorsOfPatient(string patientEmail)
        {
            var patient=await GetPatientByEmail(patientEmail);
            var result = await (from dp in _applicationContext.DoctorPatients 
                                where dp.PatientId == patient.Id
                                join p in _applicationContext.Doctors on dp.DoctorId equals p.Id
                                select p).ToListAsync();
            return result;
        }
        public async Task<bool>UpdateProfile(string patientEmail,UserDto dto)
        {
            try
            {
                var patient = await GetPatientByEmail(patientEmail);
                patient.Name = dto.Name;
                patient.Mobile = dto.Mobile;
                _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                return false;
            }

        }
    }
}
