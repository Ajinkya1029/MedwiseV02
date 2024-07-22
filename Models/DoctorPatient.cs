using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedwiseBackend.Models
{
    public class DoctorPatient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DoctorId     { get; set; }   
        public Doctors Doctors { get; set; }    
        public int PatientId { get; set; }
        public Patients Patients { get; set; }

    }
}
