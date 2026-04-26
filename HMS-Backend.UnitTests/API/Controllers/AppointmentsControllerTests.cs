using FluentAssertions;
using HMS_Backend.API.Controllers;
using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Appointment;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HMS_Backend.UnitTests.API.Controllers;

public class AppointmentsControllerTests
{
	[Fact]
	public async Task GetById_WhenAppointmentDoesNotExist_ReturnsNotFound()
	{
		// Arrange
		var appointmentId = Guid.NewGuid();
		var serviceMock = new Mock<IAppointmentService>();
		serviceMock
			.Setup(s => s.GetByIdAsync(appointmentId, It.IsAny<CancellationToken>()))
			.ReturnsAsync((AppointmentResponseDto?)null);

		var controller = new AppointmentsController(serviceMock.Object);

		// Act
		var result = await controller.GetById(appointmentId, CancellationToken.None);

		// Assert
		result.Should().BeOfType<NotFoundObjectResult>();

		var notFound = result as NotFoundObjectResult;
		notFound!.Value.Should().BeOfType<ApiResponse>();

		var response = notFound.Value as ApiResponse;
		response!.Success.Should().BeFalse();
		response.Message.Should().Contain("not found");
	}

	[Fact]
	public async Task GetById_WhenAppointmentExists_ReturnsOkWithAppointment()
	{
		// Arrange
		var appointmentId = Guid.NewGuid();
		var appointment = new AppointmentResponseDto
		{
			Id = appointmentId,
			PatientId = Guid.NewGuid(),
			PatientName = "Test Patient",
			DoctorId = Guid.NewGuid(),
			DoctorName = "Test Doctor",
			DoctorSpecialization = "Cardiology",
			AppointmentDate = DateTime.UtcNow.Date,
			StartTime = new TimeSpan(10, 0, 0),
			EndTime = new TimeSpan(10, 30, 0),
			Status = "Scheduled",
			CreatedAt = DateTime.UtcNow
		};

		var serviceMock = new Mock<IAppointmentService>();
		serviceMock
			.Setup(s => s.GetByIdAsync(appointmentId, It.IsAny<CancellationToken>()))
			.ReturnsAsync(appointment);

		var controller = new AppointmentsController(serviceMock.Object);

		// Act
		var result = await controller.GetById(appointmentId, CancellationToken.None);

		// Assert
		result.Should().BeOfType<OkObjectResult>();

		var ok = result as OkObjectResult;
		ok!.Value.Should().BeOfType<ApiResponse<AppointmentResponseDto>>();

		var response = ok.Value as ApiResponse<AppointmentResponseDto>;
		response!.Success.Should().BeTrue();
		response.Data.Should().NotBeNull();
		response.Data!.Id.Should().Be(appointmentId);
	}
}
