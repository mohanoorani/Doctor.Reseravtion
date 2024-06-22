using Doctor.Reservation.Domain;
using Doctor.Reservation.Domain.Entities;
using Doctor.Reservation.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace Doctor.Reservation.Unit.Test;

public class TimeBoxTests
{
    [Fact]
    public void Create_ShouldReturnTimeBox_WhenValidParameters()
    {
        const DayOfWeek day = DayOfWeek.Monday;
        const int hour = 10;
        const int count = 3;
        const int reservedCount = 0;

        var timeBox = TimeBox.Create(day, hour, count);

        timeBox.Should().NotBeNull();
        timeBox.Day.Should().Be(day);
        timeBox.Hour.Should().Be(hour);
        timeBox.TotalCount.Should().Be(count);
        timeBox.ReservedCount.Should().Be(reservedCount);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenDayOfWeekIsInvalid()
    {
        var act = () => TimeBox.Create((DayOfWeek)10, 10, 5);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_ShouldThrowException_WhenTotalCountIsZeroOrNegative(int totalCount)
    {
        var act = () => TimeBox.Create(DayOfWeek.Monday, 10, totalCount);

        act.Should().Throw<TotalCountCanNotBeZeroException>().WithMessage(Constants.TotalCountCanNotBeZeroOrNegativeMessage);
    }

    [Fact]
    public void Create_ShouldThrowDoctorUnavailableException_WhenReserveDateIsFriday()
    {
        var act = () => TimeBox.Create(DayOfWeek.Friday, 10, 3);
        act.Should().Throw<DoctorUnavailableException>().WithMessage(Constants.DoctorUnavailableMessage);
    }

    [Fact]
    public void Create_ShouldThrowDoctorUnavailableException_WhenReserveDateIsThursdayAfternoon()
    {
        var act = () => TimeBox.Create(DayOfWeek.Thursday, 18, 3);
        act.Should().Throw<DoctorUnavailableException>().WithMessage(Constants.DoctorUnavailableMessage);
    }

    [Fact]
    public void Reserve_ShouldThrowReservationTimeLimitException_WhenShouldReserveTwoHoursBefore()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 3);
        var reserveTime = GetFirstStartWeekDate(9);

        var act = () => timeBox.Reserve(reserveTime);
        act.Should().Throw<ReservationTimeLimitException>().WithMessage(Constants.ReservationTimeLimitMessage);
    }

    [Fact]
    public void Reserve_ShouldIncreaseReserveCount_WhenReservationIsSuccessful()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 3);
        var reserveTime = GetFirstStartWeekDate(7);

        timeBox.Reserve(reserveTime);

        timeBox.ReservedCount.Should().Be(1);
    }

    [Fact]
    public void Reserve_ShouldThrowNoReservationAvailableException_WhenNoReservationsLeft()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 1);
        var reserveTime = GetFirstStartWeekDate(7);
        timeBox.Reserve(reserveTime);

        var act = () => timeBox.Reserve(reserveTime);
        act.Should().Throw<NoReservationAvailableException>().WithMessage(Constants.NoReservationAvailableMessage);
    }

    [Fact]
    public void CancelReservation_ShouldDecreaseReserveCount()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 1);
        var reserveTime = GetFirstStartWeekDate(7);
        timeBox.Reserve(reserveTime);

        timeBox.CancelReservation();

        timeBox.ReservedCount.Should().Be(0);
    }

    private IClock GetFirstStartWeekDate(int hour)
    {
        var today = DateTime.Now;
        while (true)
        {
            if(today.DayOfWeek == DayOfWeek.Saturday)
                break;
            today = today.AddDays(1);
        }

        today = new DateTime(today.Year, today.Month, today.Day, hour, 0, 0);
        var mockClock = new Mock<IClock>();

        mockClock.Setup(c => c.Now).Returns(today);
        return mockClock.Object;
    }
}