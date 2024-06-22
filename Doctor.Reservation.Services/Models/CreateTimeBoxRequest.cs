namespace Doctor.Reservation.Services.Models;

public record CreateTimeBoxRequest(DayOfWeek Day, int Hour, int TotalCount);