using MedwiseBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace MedwiseBackend.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext>option):base(option) { }  
        public DbSet<Patients> Patients { get; set; }   
        public DbSet<Doctors>  Doctors { get; set; }

        public DbSet<Admin>Admin {  get; set; } 
        public DbSet<Hospitals> Hospitals { get; set; }
        public DbSet<HospitalDoctor> HospitalDoctors { get;set; }
        public DbSet<HospitalPatient>   HospitalPatients    { get; set; }   
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            //Hospital Patient
            modelBuilder.Entity<HospitalPatient>().HasKey(p => new { p.PatientId, p.HospitalId });
            modelBuilder.Entity<HospitalPatient>().HasOne(p => p.Patients).WithMany(p => p.Hospitals).HasForeignKey(p => p.PatientId);
            modelBuilder.Entity<HospitalPatient>().HasOne(p=>p.Hospitals).WithMany(p=>p.Patients).HasForeignKey(p=>p.HospitalId);

            //Hospital Doctor
            modelBuilder.Entity<HospitalDoctor>().HasKey(p => new { p.DoctorId, p.HospitalId });
            modelBuilder.Entity<HospitalDoctor>().HasOne(p => p.Hospitals).WithMany(p=>p.Doctors).HasForeignKey(p=>p.HospitalId);
            modelBuilder.Entity<HospitalDoctor>().HasOne(p => p.Doctors).WithMany(p => p.Hospitals).HasForeignKey(p => p.DoctorId);

            //Doctor Patient
            modelBuilder.Entity<DoctorPatient>().HasKey(p => new { p.PatientId, p.DoctorId });
            modelBuilder.Entity<DoctorPatient>().HasOne(p => p.Doctors).WithMany(p => p.Patients).HasForeignKey(p => p.DoctorId);
            modelBuilder.Entity<DoctorPatient>().HasOne(p => p.Patients).WithMany(p => p.Doctors).HasForeignKey(p => p.PatientId);

            modelBuilder.Entity<Admin>().HasOne(p => p.Hospital).WithOne(p => p.Admin).HasForeignKey<Hospitals>(p => p.AdminId);
            modelBuilder.Entity<Hospitals>().HasOne(p => p.Admin).WithOne(p => p.Hospital).HasForeignKey<Admin>(p => p.HospitalId);

        }

    }
}
