using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Services
{
    /// <summary>
    /// Pruebas unitarias para la clase <see cref="RentalService"/>.
    /// </summary>
    public class RentalServiceTests
    {
        /// <summary>
        /// Verifica que <see cref="RentalService.RentVehicleAsync"/> retorne null cuando el vehículo no está disponible.
        /// </summary>
        /// <returns>Una tarea que representa la operación de prueba. El resultado es nulo si el vehículo no está disponible.</returns>
        [Fact]
        public async Task RentVehicleAsyncReturnsNullWhenVehicleNotAvailable()
        {
            // Arrange
            var mockRepo = new Mock<IRentalRepository>(MockBehavior.Strict);
            mockRepo
                .Setup(r => r.IsVehicleAvailableAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            var service = new RentalService(mockRepo.Object);

            var request = new RentVehicleRequest
            {
                VehicleId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Renter = "Test Renter"
            };

            // Act
            var result = await service.RentVehicleAsync(request);

            // Assert
            Assert.Null(result);
        }
    }
}
