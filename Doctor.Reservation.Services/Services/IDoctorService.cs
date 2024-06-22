using Doctor.Reservation.Services.Models;

namespace Doctor.Reservation.Services.Services;

public interface IDoctorService
{
    void Reserve(ReserveRequest request, CancellationToken cancellationToken);
    void CancelReservation(CancelReservationRequest request, CancellationToken cancellationToken);
}