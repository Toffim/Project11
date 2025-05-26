using Microsoft.EntityFrameworkCore;
using Tutorial5.Data;
using Tutorial5.DTOs;
using Tutorial5.Models;

namespace Tutorial5.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task AddNewPrescriptionAsync(PrescriptionRequestDTO request)
    {
        if (request.Medicaments == null || request.Medicaments.Count > 10)
            throw new ArgumentException("Prescription cannot include more than 10 medicaments.");

        if (request.DueDate < request.Date)
            throw new ArgumentException("Error - DueDate must be greater than Date.");

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PatientId == request.Patient.PatientId);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                BirthDate = request.Patient.BirthDate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.DoctorId == request.DoctorId);

        if (doctor == null)
            throw new ArgumentException($"Doctor with Id {request.DoctorId} not found.");

        // Check if medicaments exist
        var medicamentIds = request.Medicaments.Select(m => m.MedicamentId).ToList();
        var medicamentsInDb = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.MedicamentId))
            .ToListAsync();

        if (medicamentsInDb.Count != medicamentIds.Count)
            throw new ArgumentException("One or more medicaments do not exist.");

        // Add prescription to db
        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            PatientId = patient.PatientId,
            DoctorId = doctor.DoctorId,
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        // The association table update so we got all medicaments in there
        foreach (var med in request.Medicaments)
        {
            var presMed = new PrescriptionMedicament
            {
                PrescriptionId = prescription.PrescriptionId,
                MedicamentId = med.MedicamentId,
                Dose = med.Dose,
                Details = med.Description
            };
            _context.PrescriptionMedicaments.Add(presMed);
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task<PatientInfoDTO> GetPatientInfoAsync(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.PatientId == patientId);

        if (patient == null) 
            return null;

        return new PatientInfoDTO()
        {
            PatientId = patient.PatientId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDTO
                {
                    PrescriptionId = p.PrescriptionId,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDTO
                    {
                        DoctorId = p.Doctor.DoctorId,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                        Email = p.Doctor.Email
                    },
                    Medicaments = p.PrescriptionMedicaments
                        .Select(pm => new MedicamentDTO
                        {
                            MedicamentId = pm.MedicamentId,
                            Name = pm.Medicament.Name,
                            Description = pm.Details,
                            Dose = pm.Dose
                        }).ToList()
                }).ToList()
        };
    }
}