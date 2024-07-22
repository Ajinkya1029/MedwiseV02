using Microsoft.EntityFrameworkCore;
using MedwiseBackend.Interfaces;
using System.Threading.Tasks;
using MedwiseBackend.Models;
using MedwiseBackend.Data;
using MedwiseBackend.Dto;
namespace MedwiseBackend.Repository
{
    public class DoctorRepository : IDoctorRespository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IPatientRepository _patientRepository;
        private readonly IHospitalRepository _hospitalRepository;


        public DoctorRepository(ApplicationContext applicationContext, IPatientRepository patientRepository,IHospitalRepository hospitalRepository)
        {
            _applicationContext = applicationContext;
            _patientRepository = patientRepository;
            _hospitalRepository = hospitalRepository;
        }
        public async Task<Doctors> GetDoctorByEmail(string email)
        {
            var doctor = await _applicationContext.Doctors.Where(p => p.Email == email).FirstOrDefaultAsync();
            return doctor;
        }
        public async Task<ICollection<Doctors>> GetAllDoctor()
        {
            return await _applicationContext.Doctors.OrderBy(x => x.Name).ToListAsync();
        }
        public async Task<ICollection<Doctors>> GetAllDoctorByCategory(string category)
        {
               return await _applicationContext.Doctors.Where(p => p.Category == category).OrderBy(x => x.Name).ToListAsync();
        }
        public async Task<bool> AddPatient(string patientEmail, string doctorEmail,int hospitalId)
        {
            var patient = await _patientRepository.GetPatientByEmail(patientEmail);
            if (patient == null)
            {
                return false;
            }

            var doctor = await GetDoctorByEmail(doctorEmail);
            var hospital=await _hospitalRepository.GetHospitalById(hospitalId); 

            DoctorPatient doctorPatient = new DoctorPatient
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Doctors = doctor,
                Patients = patient
            };
            HospitalPatient hospitalPatient = new HospitalPatient
            {
                PatientId = patient.Id,
                Patients=patient,
                HospitalId = hospitalId,
                Hospitals=hospital
            };
            _applicationContext.DoctorPatients.Add(doctorPatient);
            _applicationContext.HospitalPatients.Add(hospitalPatient);
            await _applicationContext.SaveChangesAsync();
            return true;

        }

        public async Task<ICollection<Patients>> GetAllPatientOfDoctor(string doctorEmail)
        {
            var doctor = await GetDoctorByEmail(doctorEmail);
            var result = await (from dp in _applicationContext.DoctorPatients
                                where dp.DoctorId == doctor.Id
                                join p in _applicationContext.Patients on dp.PatientId equals p.Id
                                select p).ToListAsync();
            return result;
        }

        public async Task<bool>UpdateProfile(string doctorEmail,UserDto dto)
        {
            try
            {

                var doctor = await GetDoctorByEmail(doctorEmail);
                doctor.Name = dto.Name;
                doctor.Mobile = dto.Mobile;
                /*doctor.Password = dto.Password;*/
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

    }
}
