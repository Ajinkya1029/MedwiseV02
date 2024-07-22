using MedwiseBackend.Data;
using MedwiseBackend.Interfaces;
using MedwiseBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MedwiseBackend.Repository
{
    public class HospitalRepository:IHospitalRepository
    {
        private readonly ApplicationContext _applicationContext;
        public HospitalRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<ICollection<Hospitals>> GetAllHospitalAsync()
        {
            var result =await _applicationContext.Hospitals.OrderBy(p=>p.Name).ToListAsync();
            return result;
        }
        public async Task<ICollection<Hospitals>> GetAllHospitalByCityAsync(string city)
        {
            var result=await _applicationContext.Hospitals.Where(p=>p.City==city).OrderBy(p=>p.City).ToListAsync();
            return result;
        }

        public async Task<Hospitals>GetHospitalById(int Id)
        {
            var result = await _applicationContext.Hospitals.Where(p => p.Id == Id).FirstOrDefaultAsync();
            return result;
        }
    }
}
