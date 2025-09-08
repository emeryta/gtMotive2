using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Controllers
{
    public sealed class RentalsApiControllerTests
    {
        [Fact]
        public async Task CreateRentalModelInvalidoRetornaBadRequest()
        {
            // Arrange
            var mockRepo = new Mock<RentalRepository>(MockBehavior.Strict);
            var rentalService = new RentalService(mockRepo.Object);
            var controller = new RentalsController(rentalService);

            controller.ModelState.AddModelError("VehicleId", "Required");

            var rental = new RentVehicleRequest
            {
                VehicleId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Renter = "Test Renter"
            };

            // Act
            var result = await controller.RentVehicle(rental);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
