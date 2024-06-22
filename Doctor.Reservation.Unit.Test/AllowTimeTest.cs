using Doctor.Reservation.Domain.Entities;
using FluentAssertions;

namespace Doctor.Reservation.Unit.Test;

public class AllowTimeTests
{
    [Fact]
    public void Create_ShouldReturnAllowTime_WhenValidParameters()
    {
        var fromTime = new TimeOnly(10, 0);
        var toTime = new TimeOnly(15, 0);
        var allowTime = new AllowTime(fromTime, toTime);

        allowTime.Should().NotBeNull();
        allowTime.FromTime.Should().Be(fromTime);
        allowTime.ToTime.Should().Be(toTime);
    }
}