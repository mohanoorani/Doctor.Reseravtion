using Doctor.Reservation.Services.Models;
using Doctor.Reservation.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doctor.Reservation.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class TimeBoxController : ControllerBase
{
    private readonly ITimeBoxService _timeBoxService;

    public TimeBoxController(ITimeBoxService timeBoxService)
    {
        _timeBoxService = timeBoxService;
    }

    [HttpPost]
    public IActionResult Create(CreateTimeBoxRequest request, CancellationToken cancellationToken)
    {
        _timeBoxService.Create(request, cancellationToken);

        return Ok();
    }

    [HttpGet]
    public IActionResult Get([FromQuery] GetTimeBoxRequest request, CancellationToken cancellationToken)
    {
        var response = _timeBoxService.Get(request, cancellationToken);

        return Ok(response);
    }
}