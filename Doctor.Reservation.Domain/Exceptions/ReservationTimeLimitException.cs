namespace Doctor.Reservation.Domain.Exceptions;

public class ReservationTimeLimitException : DomainException
{
    public override string Message { get; } = Constants.ReservationTimeLimitMessage;
}