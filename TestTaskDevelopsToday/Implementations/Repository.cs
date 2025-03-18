using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using TestTaskDevelopsToday.Abstractions;
using TestTaskDevelopsToday.Data;
using TestTaskDevelopsToday.DTOs;
using TestTaskDevelopsToday.Entities;
using TestTaskDevelopsToday.Utilities;
using static TestTaskDevelopsToday.Data.SqlCommands;

namespace TestTaskDevelopsToday.Implementations;

public class Repository(SqlConnectionFactory sqlConnectionFactory) : IRepository
{
    public async Task InitializeDatabase()
    {
        await using var connection = sqlConnectionFactory.CreateConnection();

        await connection.ExecuteAsync(CreateDatabase);

        await connection.ExecuteAsync(CreateTable);
    }

    public async Task BulkInsertTrips(List<Trip> trips)
    {
        await using var connection = sqlConnectionFactory.CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync("USE TripsDB");

        using var bulkCopy = new SqlBulkCopy(connection);
        
        bulkCopy.DestinationTableName = "Trips";
        bulkCopy.BatchSize = 1000;
        
        bulkCopy.ColumnMappings.Add("PickupDateTime", "PickupDateTime");
        bulkCopy.ColumnMappings.Add("DropoffDateTime", "DropoffDateTime");
        bulkCopy.ColumnMappings.Add("PassengerCount", "PassengerCount");
        bulkCopy.ColumnMappings.Add("TripDistance", "TripDistance");
        bulkCopy.ColumnMappings.Add("StoreAndFwdFlag", "StoreAndFwdFlag");
        bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
        bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
        bulkCopy.ColumnMappings.Add("FareAmount", "FareAmount");
        bulkCopy.ColumnMappings.Add("TipAmount", "TipAmount");
        
        var dataTable = new DataTable();
        dataTable.Columns.Add("PickupDateTime", typeof(DateTime));
        dataTable.Columns.Add("DropoffDateTime", typeof(DateTime));
        dataTable.Columns.Add("PassengerCount", typeof(int));
        dataTable.Columns.Add("TripDistance", typeof(decimal));
        dataTable.Columns.Add("StoreAndFwdFlag", typeof(string));
        dataTable.Columns.Add("PULocationID", typeof(int));
        dataTable.Columns.Add("DOLocationID", typeof(int));
        dataTable.Columns.Add("FareAmount", typeof(decimal));
        dataTable.Columns.Add("TipAmount", typeof(decimal));

        foreach (var trip in trips)
        {
            dataTable.Rows.Add(
                trip.PickupDateTime,
                trip.DropoffDateTime,
                trip.PassengerCount,
                trip.TripDistance,
                trip.StoreAndFwdFlag,
                trip.PULocationID,
                trip.DOLocationID,
                trip.FareAmount,
                trip.TipAmount
            );
        }

        await bulkCopy.WriteToServerAsync(dataTable);
    }

    public async Task<LocationWithHighestAverageTipDto> GetLocationWithHighestAverageTipAsync()
    {
        await using var connection = sqlConnectionFactory.CreateConnection();

        var result =
            await connection.QuerySingleOrDefaultAsync<LocationWithHighestAverageTipDto>(
                FindLocationWithHighestAverageTip);

        return result;
    }

    public async Task<List<TripDto>> GetTop100LongestTripDistancesAsync()
    {
        await using var connection = sqlConnectionFactory.CreateConnection();

        var result = await connection.QueryAsync<TripDto>(SelectWithLongestTripDistance);

        return result.ToList();
    }

    public async Task<List<TripDto>> GetTop100LongestTripTimesAsync()
    {
        await using var connection = sqlConnectionFactory.CreateConnection();

        var result = await connection.QueryAsync<TripDto>(SelectWithLongestTripTime);

        return result.ToList();
    }

    public async Task<List<TripDto>> GetTripsByPickupLocationAsync(int pickUpLocationId)
    {
        await using var connection = sqlConnectionFactory.CreateConnection();

        var result =
            await connection.QueryAsync<TripDto>(SearchTripsByPickUpLocation, new { PULocationID = pickUpLocationId });

        return result.ToList();
    }
}