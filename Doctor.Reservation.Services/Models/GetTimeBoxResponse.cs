namespace Doctor.Reservation.Services.Models;

public record GetTimeBoxResponse
{
    public string Day { get; set; }
    public int Hour { get; set; }
    public int TotalCount { get; set; }
    public int ReservedCount { get; set; }
}