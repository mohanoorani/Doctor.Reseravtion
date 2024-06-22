using Doctor.Reservation.Services.Models;

namespace Doctor.Reservation.Services.Services;

public interface ITimeBoxService
{
    void Create(CreateTimeBoxRequest request, CancellationToken cancellationToken);
    List<GetTimeBoxResponse>? Get(GetTimeBoxRequest request, CancellationToken cancellationToken);
}