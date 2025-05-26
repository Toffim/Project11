using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }

    protected DatabaseContext() { }

    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.DoctorId);
            entity.Property(d => d.FirstName).HasMaxLength(100);
            entity.Property(d => d.LastName).HasMaxLength(100);
            entity.Property(d => d.Email).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.PatientId);
            entity.Property(p => p.FirstName).HasMaxLength(100);
            entity.Property(p => p.LastName).HasMaxLength(100);
        });
        
        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(p => p.PrescriptionId);
            entity.HasOne(p => p.Patient)
                .WithMany()
                .HasForeignKey(p => p.PatientId);

            entity.HasOne(p => p.Doctor)
                .WithMany()
                .HasForeignKey(p => p.DoctorId);
        });

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.HasKey(m => m.MedicamentId);
            entity.Property(m => m.Name).HasMaxLength(100);
            entity.Property(m => m.Description).HasMaxLength(100);
            entity.Property(m => m.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<PrescriptionMedicament>(entity =>
        {
            entity.HasKey(pm => new { pm.PrescriptionId, pm.MedicamentId });

            entity.HasOne(pm => pm.Prescription)
                .WithMany()
                .HasForeignKey(pm => pm.PrescriptionId);

            entity.HasOne(pm => pm.Medicament)
                .WithMany()
                .HasForeignKey(pm => pm.MedicamentId);

            entity.Property(pm => pm.Details).HasMaxLength(100);
        });
    }
}