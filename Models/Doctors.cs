using System.ComponentModel.DataAnnotations;

namespace MedwiseBackend.Models
{
    public class Doctors:User
    {
        public string? Mobile {  get; set; }     
        public string? Review    { get; set; }   
        public string? Category {  get; set; }   
        public ICollection<DoctorPatient>? Patients { get; set; }
        public ICollection<HospitalDoctor>? Hospitals{ get;set; } 

    }
}
