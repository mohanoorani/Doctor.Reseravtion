namespace Doctor.Reservation.Domain.Exceptions;

public class ReservationTimeLimitException : Exception
{
    public override string Message { get; } = Constants.ReservationTimeLimitMessage;
}