using System.ComponentModel.DataAnnotations;

namespace MedwiseBackend.Models
{
    public class Admin:User
    {
        public Hospitals Hospital { get; set; }
        public int HospitalId { get; set; }

    }
}
