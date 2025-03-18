using TestTaskDevelopsToday.DTOs;
using TestTaskDevelopsToday.Entities;

namespace TestTaskDevelopsToday.Abstractions;

public interface IRepository
{
    Task InitializeDatabase();

    Task BulkInsertTrips(List<Trip> trips);

    Task<LocationWithHighestAverageTipDto> GetLocationWithHighestAverageTipAsync();

    Task<List<TripDto>> GetTop100LongestTripDistancesAsync();

    Task<List<TripDto>> GetTop100LongestTripTimesAsync();

    Task<List<TripDto>> GetTripsByPickupLocationAsync(int pickUpLocationId);
} 