using Microsoft.EntityFrameworkCore;
using MedwiseBackend.Interfaces;
using System.Threading.Tasks;
using MedwiseBackend.Data;
using MedwiseBackend.Models;
using MedwiseBackend.Dto;
namespace MedwiseBackend.Repository
{
    public class AdminRepository:IAdminRepository
    {
        private readonly ApplicationContext _applicationContext;
        
        public AdminRepository(ApplicationContext applicationContext) { 
        _applicationContext = applicationContext;
        }
        public async Task<Admin>GetAdminByEmail(string adminEmail) {
        var result=await _applicationContext.Admin.Where(p=>p.Email== adminEmail).FirstOrDefaultAsync();
            return result;
        }
        public async Task<ICollection<Doctors>> GetDoctorList(string adminEmail)
        {
            var admin = await GetAdminByEmail(adminEmail);
            var result = await (from dp in _applicationContext.HospitalDoctors
                                where dp.HospitalId == admin.HospitalId
                                join p in _applicationContext.Doctors on dp.DoctorId equals p.Id
                                select p).ToListAsync();
            return result;
        }
        public async Task<ICollection<Patients>>GetPatientList(string adminEmail)
        {
            var admin=await GetAdminByEmail(adminEmail);
            var result = await (from dp in _applicationContext.HospitalPatients
                                where dp.HospitalId == admin.HospitalId
                                join p in _applicationContext.Patients on dp.PatientId equals p.Id
                                select p).ToListAsync();
            return result;
        }
        
        public async Task<bool>RegisterDoctor(string adminEmail,UserDto dto)
        {
            try
            {
                var admin = await GetAdminByEmail(adminEmail);
                Doctors doctors = new Doctors()
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = dto.Password,
                   Roles="doctor"
                };
                _applicationContext.Doctors.Add(doctors);
                await _applicationContext.SaveChangesAsync();

                HospitalDoctor hospitalDoctor = new HospitalDoctor()
                {
                    HospitalId = admin.HospitalId,
                    DoctorId = doctors.Id,
                };
                _applicationContext.HospitalDoctors.Add(hospitalDoctor);
                _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
        public async Task<bool>RegisterAdmin(RegisterDto admin)
        {
            try
            {

                Admin data = new Admin
                {
                    Name = admin.Name,
                    Email = admin.Email,
                    Password = admin.Password,
                    HospitalId = admin.entityId,

                };
                _applicationContext.Admin.Add(data);
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}
