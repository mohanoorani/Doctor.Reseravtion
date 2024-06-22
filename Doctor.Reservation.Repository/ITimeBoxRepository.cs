using Doctor.Reservation.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Doctor.Reservation.Repository;

public interface ITimeBoxRepository
{
    void Set(DayOfWeek day, List<TimeBox> timeBoxes);
    List<TimeBox>? Get(DayOfWeek day);
}

public class TimeBoxRepository : ITimeBoxRepository
{

    private readonly IMemoryCache _memoryCache;

    public TimeBoxRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set(DayOfWeek day, List<TimeBox> timeBoxes)
    {
        _memoryCache.Set(day, timeBoxes);
    }

    public List<TimeBox>? Get(DayOfWeek day)
    {
        return _memoryCache.Get<List<TimeBox>>(day);
    }
}