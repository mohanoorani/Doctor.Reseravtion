namespace Doctor.Reservation.Services.Models;

public record CancelReservationRequest(DayOfWeek Day, int Hour);
