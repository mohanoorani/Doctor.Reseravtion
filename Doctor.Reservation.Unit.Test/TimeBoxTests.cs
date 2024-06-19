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

        var timeBox = TimeBox.Create(day, hour, count);

        timeBox.Should().NotBeNull();
        timeBox.Day.Should().Be(day);
        timeBox.Hour.Should().Be(hour);
        timeBox.Count.Should().Be(count);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenDayOfWeekIsInvalid()
    {
        var act = () => TimeBox.Create((DayOfWeek)10, 10, 0);

        act.Should().Throw<ArgumentOutOfRangeException>();
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
        var reserveTime = GetTodayReserveDate(hour: 9);

        var act = () => timeBox.Reserve(reserveTime);
        act.Should().Throw<ReservationTimeLimitException>().WithMessage(Constants.ReservationTimeLimitMessage);
    }

    [Fact]
    public void Reserve_ShouldDecreaseCount_WhenReservationIsSuccessful()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 3);
        var reserveTime = GetTodayReserveDate(hour: 7);

        timeBox.Reserve(reserveTime);

        timeBox.Count.Should().Be(2);
    }

    [Fact]
    public void Reserve_ShouldThrowNoReservationAvailableException_WhenNoReservationsLeft()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 1);
        var reserveTime = GetTodayReserveDate(hour: 7);
        timeBox.Reserve(reserveTime);

        var act = () => timeBox.Reserve(reserveTime);
        act.Should().Throw<NoReservationAvailableException>().WithMessage(Constants.NoReservationAvailableMessage);
    }

    [Fact]
    public void CancelReservation_ShouldIncreaseCount()
    {
        var timeBox = TimeBox.Create(DateTime.Now.DayOfWeek, 10, 1);
        var reserveTime = GetTodayReserveDate(hour: 7);
        timeBox.Reserve(reserveTime);

        timeBox.CancelReservation();

        timeBox.Count.Should().Be(1);
    }

    private IClock GetTodayReserveDate(int hour)
    {
        var dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, 0, 0);
        var mockClock = new Mock<IClock>();

        mockClock.Setup(c => c.Now).Returns(dateTime);
        return mockClock.Object;
    }
}

