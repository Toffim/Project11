using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

[Table("Prescription")]
public class Prescription
{
    [Key]
    public int PrescriptionId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public int PatientId { get; set; }
    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }

    public int DoctorId { get; set; }
    [ForeignKey("DoctorId")]
    public virtual Doctor Doctor { get; set; }
}