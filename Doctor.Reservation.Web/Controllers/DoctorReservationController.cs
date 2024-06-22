using Doctor.Reservation.Services.Models;
using Doctor.Reservation.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Reservation.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorReservationController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorReservationController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }
    [HttpPut("Reserve")]
    public IActionResult Reserve(ReserveRequest request, CancellationToken cancellationToken)
    {
        _doctorService.Reserve(request, cancellationToken);
        
        return NoContent();
    }

    [HttpPut("Cancel")]
    public IActionResult CancelReservation(CancelReservationRequest request, CancellationToken cancellationToken)
    {
        _doctorService.CancelReservation(request, cancellationToken);
        
        return NoContent();
    }
}