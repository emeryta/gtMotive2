using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Models;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Services
{
    public class RentalServiceIntegrationTests
    {
        [Fact]
        public async Task RentVehicleAsyncCreatesRentalWhenVehicleIsAvailable()
        {
            var rentalRepository = new InMemoryRentalRepository();
            var service = new RentalService(rentalRepository);
            var request = CreateTestRequest();

            var result = await service.RentVehicleAsync(request);

            Assert.NotNull(result);
            Assert.Equal(request.VehicleId, result.VehicleId);
            Assert.Equal(request.Renter, result.Renter);
            Assert.True(rentalRepository.ExistsRental(result.Id));
        }

        [Fact]
        public async Task RentVehicleAsyncReturnsNullWhenVehicleIsNotAvailable()
        {
            var rentalRepository = new InMemoryRentalRepository();
            rentalRepository.SetVehicleUnavailable(true);
            var service = new RentalService(rentalRepository);
            var request = CreateTestRequest();

            var result = await service.RentVehicleAsync(request);

            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnVehicleAsyncRemovesRentalById()
        {
            var rentalRepository = new InMemoryRentalRepository();
            var service = new RentalService(rentalRepository);
            var request = CreateTestRequest();

            var rental = await service.RentVehicleAsync(request);
            Assert.NotNull(rental);

            var deleted = await service.ReturnVehicleAsync(rental.Id);

            Assert.True(deleted);
            Assert.False(rentalRepository.ExistsRental(rental.Id));
        }

        [Fact]
        public async Task ReturnVehicleByVehicleIdAsyncRemovesRentalByVehicleId()
        {
            var rentalRepository = new InMemoryRentalRepository();
            var service = new RentalService(rentalRepository);
            var vehicleId = Guid.NewGuid();
            var request = CreateTestRequest(vehicleId);

            var rental = await service.RentVehicleAsync(request);
            Assert.NotNull(rental);

            var deleted = await service.ReturnVehicleByVehicleIdAsync(vehicleId);

            Assert.True(deleted);
            Assert.False(rentalRepository.ExistsRentalByVehicleId(vehicleId));
        }

        [Fact]
        public async Task GetAllRentalsAsyncReturnsAllRentals()
        {
            var rentalRepository = new InMemoryRentalRepository();
            var service = new RentalService(rentalRepository);

            var request1 = CreateTestRequest();
            var request2 = CreateTestRequest();

            var rental1 = await service.RentVehicleAsync(request1);
            var rental2 = await service.RentVehicleAsync(request2);

            var allRentals = await service.GetAllRentalsAsync();

            Assert.Contains(allRentals, r => r.Id == rental1.Id);
            Assert.Contains(allRentals, r => r.Id == rental2.Id);
            Assert.Equal(2, allRentals.Count);
        }

        private static RentVehicleRequest CreateTestRequest(Guid? vehicleId = null)
        {
            return new RentVehicleRequest
            {
                VehicleId = vehicleId ?? Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Renter = "Integración Test"
            };
        }
    }
}
