using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedwiseBackend.Models
{
    public class HospitalDoctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int HospitalId    { get; set; }   
        public Hospitals Hospitals { get; set; }
        public int DoctorId     { get; set; }   
        public Doctors Doctors { get; set; }    
    }
}
