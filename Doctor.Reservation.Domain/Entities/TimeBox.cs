using Doctor.Reservation.Domain.Exceptions;

namespace Doctor.Reservation.Domain.Entities;

public class TimeBox
{
    private static readonly object LockObject = new();

    private TimeBox(DayOfWeek day, int hour, int totalCount)
    {
        Day = day;
        Hour = hour;
        TotalCount = totalCount;
        ReservedCount = 0;
    }

    public DayOfWeek Day { get; private set; }
    public int Hour { get; }
    public int TotalCount { get; private set; }
    public int ReservedCount { get; private set; }

    public static TimeBox Create(DayOfWeek day, int hour, int totalCount)
    {
        if (totalCount < 1)
            throw new TotalCountCanNotBeZeroException();

        var allowedTime = GetWeekTiming(day);

        if (allowedTime.FromTime.Hour <= hour && allowedTime.ToTime.Hour >= hour)
            return new TimeBox(day, hour, totalCount);

        throw new DoctorUnavailableException();
    }

    public void Reserve(IClock currentTime)
    {
        lock (LockObject)
        {
            if (TotalCount - ReservedCount == 0)
                throw new NoReservationAvailableException();

            if (!IsReservationMinimumTimeAllowed(currentTime, Hour))
                throw new ReservationTimeLimitException();

            ReservedCount++;
        }
    }

    public void CancelReservation()
    {
        if(ReservedCount  == 0) return;

        ReservedCount--;
    }

    private static AllowTime GetWeekTiming(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Friday => throw new DoctorUnavailableException(),
            DayOfWeek.Saturday or DayOfWeek.Sunday or DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday =>
                new AllowTime(new TimeOnly(9, 0), new TimeOnly(17, 0)),
            DayOfWeek.Thursday =>
                new AllowTime(new TimeOnly(9, 0), new TimeOnly(13, 0)),
            _ => throw new ArgumentOutOfRangeException(nameof(day), day, null)
        };
    }

    private bool IsReservationMinimumTimeAllowed(IClock currentTime, int hour)
    {
        var reservationTime = new TimeOnly(hour, 0, 0);
        var minimumAllowedTime = new TimeOnly(currentTime.Now.AddHours(2).Hour, 0, 0);

        return currentTime.Now.DayOfWeek != Day || reservationTime > minimumAllowedTime;
    }
}