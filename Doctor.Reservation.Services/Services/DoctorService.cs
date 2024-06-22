using Doctor.Reservation.Domain;
using Doctor.Reservation.Domain.Entities;
using Doctor.Reservation.Repository;
using Doctor.Reservation.Services.Models;

namespace Doctor.Reservation.Services.Services;

public class DoctorService : IDoctorService
{
    private readonly ITimeBoxRepository _timeBoxRepository;
    private readonly IClock _clock;

    public DoctorService(ITimeBoxRepository timeBoxRepository, IClock clock)
    {
        _timeBoxRepository = timeBoxRepository;
        _clock = clock;
    }

    public void Reserve(ReserveRequest request, CancellationToken cancellationToken)
    {
        var timeBox = GetTimeBox(request.Day , request.Hour);

        timeBox.Reserve(_clock);
    }

    public void CancelReservation(CancelReservationRequest request, CancellationToken cancellationToken)
    {
        var timeBox = GetTimeBox(request.Day, request.Hour);

        timeBox.CancelReservation();
    }

    private TimeBox GetTimeBox(DayOfWeek day, int hour)
    {
        var timeBoxes = _timeBoxRepository.Get(day);
        if (timeBoxes is null)
        {
            throw new KeyNotFoundException(Constants.DoctorNotAvailableInThisDayMessage);
        }

        var timeBox = timeBoxes.FirstOrDefault(i => i.Hour == hour);
        if (timeBox is null)
        {
            throw new KeyNotFoundException(Constants.DoctorNotAvailableAtThisTimeMessage);
        }

        return timeBox;
    }
}