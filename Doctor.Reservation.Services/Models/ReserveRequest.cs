namespace Doctor.Reservation.Services.Models;

public record ReserveRequest(DayOfWeek Day, int Hour);
