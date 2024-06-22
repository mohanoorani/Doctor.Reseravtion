using System.Security.Cryptography.X509Certificates;
using Doctor.Reservation.Domain.Entities;
using Doctor.Reservation.Repository;
using Doctor.Reservation.Services.Models;

namespace Doctor.Reservation.Services.Services;

public class TimeBoxService : ITimeBoxService
{
    private readonly ITimeBoxRepository _timeBoxRepository;

    public TimeBoxService(ITimeBoxRepository timeBoxRepository)
    {
        _timeBoxRepository = timeBoxRepository;
    }

    public void Create(CreateTimeBoxRequest request, CancellationToken cancellationToken)
    {
        var timeBox = TimeBox.Create(request.Day, request.Hour, request.TotalCount);

        var dayTimeBoxes = _timeBoxRepository.Get(request.Day);
        if (dayTimeBoxes is null)
        {
            dayTimeBoxes = new List<TimeBox> { timeBox };
        }
        else
        {
            dayTimeBoxes.RemoveAll(i => i.Hour == request.Hour);
            dayTimeBoxes.Add(timeBox);
        }

        _timeBoxRepository.Set(timeBox.Day, dayTimeBoxes);
    }

    public List<GetTimeBoxResponse>? Get(GetTimeBoxRequest request, CancellationToken cancellationToken)
    {
        var timeBoxes = _timeBoxRepository.Get(request.Day);

        return timeBoxes?.Select(i => new GetTimeBoxResponse
        {
            Day = i.Day.ToString(),
            Hour = i.Hour,
            TotalCount = i.TotalCount,
            ReservedCount = i.ReservedCount
        }).ToList();
    }
}