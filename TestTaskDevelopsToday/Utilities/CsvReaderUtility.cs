using System.Globalization;
using TestTaskDevelopsToday.Entities;
using CsvHelper;
using CsvHelper.Configuration;

namespace TestTaskDevelopsToday.Utilities;

public static class CsvReaderUtility
{
    public static List<Trip> ReadAndTransformCsv(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim
        });
        
        var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        var records = csv.GetRecords<dynamic>().Select(row =>
            {
                DateTime pickupDate, dropoffDate;
                int passengers, pickupLoc, dropoffLoc;
                decimal distance, fare, tip;

                DateTime.TryParse(row.tpep_pickup_datetime, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out pickupDate);
                DateTime.TryParse(row.tpep_dropoff_datetime, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out dropoffDate);
                
                // Convert from EST to UTC
                pickupDate = TimeZoneInfo.ConvertTimeToUtc(pickupDate, estTimeZone);
                dropoffDate = TimeZoneInfo.ConvertTimeToUtc(dropoffDate, estTimeZone);


                int.TryParse(row.passenger_count, out passengers);
                int.TryParse(row.PULocationID, out pickupLoc);
                int.TryParse(row.DOLocationID, out dropoffLoc);

                decimal.TryParse(row.trip_distance, NumberStyles.Any, CultureInfo.InvariantCulture, out distance);
                decimal.TryParse(row.fare_amount, NumberStyles.Any, CultureInfo.InvariantCulture, out fare);
                decimal.TryParse(row.tip_amount, NumberStyles.Any, CultureInfo.InvariantCulture, out tip);

                return new Trip
                {
                    PickupDateTime = pickupDate,
                    DropoffDateTime = dropoffDate,
                    PassengerCount = passengers,
                    TripDistance = distance,
                    StoreAndFwdFlag = row.store_and_fwd_flag?.Trim() == "Y" ? "Yes" : "No",
                    PULocationID = pickupLoc,
                    DOLocationID = dropoffLoc,
                    FareAmount = fare,
                    TipAmount = tip
                };
            })
            .Where(trip => trip is { FareAmount: >= 0, TipAmount: >= 0, PassengerCount: >= 0, TripDistance: >= 0 })
            .ToList();


        return records;
    }
}