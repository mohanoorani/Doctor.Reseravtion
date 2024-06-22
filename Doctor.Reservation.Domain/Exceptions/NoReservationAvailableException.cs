namespace Doctor.Reservation.Domain.Exceptions;

public class NoReservationAvailableException : DomainException
{
    public override string Message { get; } = Constants.NoReservationAvailableMessage;
}