using MedwiseBackend.Models;

namespace MedwiseBackend.Interfaces
{
    public interface IHospitalRepository
    {
        public Task<ICollection<Hospitals>> GetAllHospitalAsync();
        public Task<ICollection<Hospitals>> GetAllHospitalByCityAsync(string city);
        public Task<Hospitals> GetHospitalById(int Id);
    }
}
