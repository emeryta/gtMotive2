using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Services
{
    public class InMemoryRentalRepository : IRentalRepository
    {
        private readonly List<Rental> _rentals = new();

        private bool _forceUnavailable;

        public void SetVehicleUnavailable(bool unavailable)
        {
            _forceUnavailable = unavailable;
        }

        public Task<bool> IsVehicleAvailableAsync(Guid vehicleId, DateTime startDate, DateTime endDate)
        {
            return Task.FromResult(!_forceUnavailable);
        }

        public Task CreateRentalAsync(Rental rental)
        {
            _rentals.Add(rental);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteRentalAsync(Guid rentalId)
        {
            var removed = _rentals.RemoveAll(r => r.Id == rentalId) > 0;
            return Task.FromResult(removed);
        }

        public Task<bool> ReturnVehicleByVehicleIdAsync(Guid vehicleId)
        {
            var removed = _rentals.RemoveAll(r => r.VehicleId == vehicleId) > 0;
            return Task.FromResult(removed);
        }

        public Task<List<Rental>> GetAllAsync()
        {
            return Task.FromResult(new List<Rental>(_rentals));
        }

        public bool ExistsRental(Guid rentalId)
        {
            return _rentals.Exists(r => r.Id == rentalId);
        }

        public bool ExistsRentalByVehicleId(Guid vehicleId)
        {
            return _rentals.Exists(r => r.VehicleId == vehicleId);
        }
    }
}
