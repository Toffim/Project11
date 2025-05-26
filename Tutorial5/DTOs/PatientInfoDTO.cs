namespace Tutorial5.DTOs;

public class PatientInfoDTO
{
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    public List<PrescriptionDTO> Prescriptions { get; set; }
}