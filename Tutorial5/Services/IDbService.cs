using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface IDbService
{
    Task AddNewPrescriptionAsync(PrescriptionRequestDTO request);
}