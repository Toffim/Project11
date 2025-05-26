using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

[PrimaryKey(nameof(PrescriptionId), nameof(MedicamentId))]
[Table("Prescription_Medicament")]
public class PrescriptionMedicament
{
    [ForeignKey(nameof(Prescription))]
    public int PrescriptionId { get; set; }
    [ForeignKey(nameof(Medicament))]
    public int MedicamentId { get; set; }
    
    [MaxLength(100)]
    public string Details { get; set; }
    public int? Dose { get; set; }

    public Prescription Prescription { get; set; }
    public Medicament Medicament { get; set; }
}