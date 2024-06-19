using Doctor.Reservation.Domain.Exceptions;

namespace Doctor.Reservation.Domain.Entities;

public class TimeBox
{
    public DayOfWeek Day { get; private set; }
    public int Hour { get; private set; }
    public int Count { get; private set; }

    private static readonly object LockObject = new();

    private TimeBox(DayOfWeek day, int hour, int count)
    {
        Day = day;
        Hour = hour;
        Count = count;
    }

    public static TimeBox Create(DayOfWeek day, int hour, int count)
    {
        var allowedTime = GetWeekTiming(day);

        if (allowedTime.FromTime.Hour <= hour && allowedTime.ToTime.Hour >= hour)
            return new TimeBox(day, hour, count);

        throw new DoctorUnavailableException();
    }

    public void Reserve(IClock currentTime)
    {
        lock (LockObject)
        {
            if (Count == 0)
                throw new NoReservationAvailableException();

            if (!IsReservationMinimumTimeAllowed(currentTime, Hour))
                throw new ReservationTimeLimitException();

            Count--;
        }
    }

    public void CancelReservation()
    {
        Count++;
    }

    private static AllowTime GetWeekTiming(DayOfWeek day) => day switch
    {
        DayOfWeek.Friday => throw new DoctorUnavailableException(),
        DayOfWeek.Saturday or DayOfWeek.Sunday or DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday =>
            new AllowTime(new TimeOnly(9, 0), new TimeOnly(17, 0)),
        DayOfWeek.Thursday =>
            new AllowTime(new TimeOnly(9, 0), new TimeOnly(13, 0)),
        _ => throw new ArgumentOutOfRangeException(nameof(day), day, null)
    };

    private bool IsReservationMinimumTimeAllowed(IClock currentTime, int hour)
    {
        var reservationTime = new TimeOnly(hour, 0, 0);
        var minimumAllowedTime = new TimeOnly(currentTime.Now.AddHours(2).Hour, 0, 0);

        return reservationTime >= minimumAllowedTime;
    }
}