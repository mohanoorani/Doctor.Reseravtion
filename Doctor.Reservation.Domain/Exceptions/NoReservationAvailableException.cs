namespace Doctor.Reservation.Domain.Exceptions;

public class NoReservationAvailableException : Exception
{
    public override string Message { get; } = Constants.NoReservationAvailableMessage;
}