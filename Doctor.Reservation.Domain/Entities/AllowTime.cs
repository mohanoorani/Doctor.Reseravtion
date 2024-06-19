namespace Doctor.Reservation.Domain.Entities;

public class AllowTime
{
    public AllowTime(TimeOnly fromTime, TimeOnly toTime)
    {
        FromTime = fromTime;
        ToTime = toTime;
    }
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
}