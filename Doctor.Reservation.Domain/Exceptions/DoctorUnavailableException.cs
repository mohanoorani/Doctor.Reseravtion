namespace Doctor.Reservation.Domain.Exceptions;

public class DoctorUnavailableException : Exception
{
    public override string Message { get; } = Constants.DoctorUnavailableMessage;
}