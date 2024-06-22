namespace Doctor.Reservation.Domain.Exceptions;

public class DoctorUnavailableException : DomainException
{
    public override string Message { get; } = Constants.DoctorUnavailableMessage;
}