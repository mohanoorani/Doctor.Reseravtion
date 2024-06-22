namespace Doctor.Reservation.Domain.Exceptions;

public class TotalCountCanNotBeZeroException : DomainException
{
    public override string Message { get; } = Constants.TotalCountCanNotBeZeroOrNegativeMessage;
}