namespace Tutorial5.DTOs;

public class PrescriptionRequestDTO
{
    public PatientDTO Patient { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentDTO> Medicaments { get; set; }
}
