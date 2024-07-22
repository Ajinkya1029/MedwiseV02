using System.ComponentModel.DataAnnotations;

namespace MedwiseBackend.Models
{
    public class Hospitals
    {
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }    
        public string? Mobile { get; set; }  
        public string? City {  get; set; }
        public  Admin? Admin { get; set; } 
        public int? AdminId { get; set; }
        public ICollection<HospitalPatient>?Patients { get; set; }   
        public ICollection<HospitalDoctor>?Doctors { get; set; }


    }
}
