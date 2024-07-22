using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedwiseBackend.Models
{
    public class HospitalPatient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }    
        public int HospitalId  { get; set; }    
        public Hospitals Hospitals { get; set; }    
        public int PatientId    { get; set; }   
        public Patients Patients { get; set; }  

    }
}
