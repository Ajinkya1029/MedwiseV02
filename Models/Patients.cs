using System.ComponentModel.DataAnnotations;

namespace MedwiseBackend.Models
{
    public class Patients:User
    { 
        public DateTime? Dob {  get; set; }  
        public string? Address {  get; set; }  
        public string? Mobile { get; set; } 

        public ICollection<DoctorPatient>? Doctors { get; set; }
        public ICollection<HospitalPatient>?Hospitals { get; set; }

    }
}
