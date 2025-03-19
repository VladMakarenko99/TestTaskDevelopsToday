using static TestTaskDevelopsToday.Utilities.PathUtility;
using TestTaskDevelopsToday.Abstractions;
using TestTaskDevelopsToday.DTOs;
using TestTaskDevelopsToday.Utilities;

namespace TestTaskDevelopsToday.Implementations;

public class UserInterfaceServiceService(IRepository repository) : IUserInterfaceService
{
    public async Task Start()
    {
        Console.WriteLine("🚀 Initializing database...");
        await repository.InitializeDatabase();
        Console.WriteLine("✅ Database initialized!");

        string csvFilePath = Path.Combine(RemoveUnwantedSegments(AppContext.BaseDirectory), "source.csv");
        string duplicatesFilePath = Path.Combine(RemoveUnwantedSegments(AppContext.BaseDirectory), "duplicates.csv");

        Console.WriteLine("📥 Reading CSV...");
        var trips = CsvReaderUtility.ReadAndTransformCsv(csvFilePath);
        Console.WriteLine($"✅ Read {trips.Count} records.");

        Console.WriteLine("🔍 Removing duplicates...");
        var (uniqueTrips, duplicateTrips) = DuplicateCleanerUtility.RemoveDuplicates(trips);
        Console.WriteLine($"✅ Found {duplicateTrips.Count} duplicates.");
        
        Console.WriteLine("📄 Saving duplicates to file...");
        await CsvWriterUtility.WriteToCsv(duplicateTrips, duplicatesFilePath);
        Console.WriteLine($"✅ Duplicates saved to {duplicatesFilePath}");

        Console.WriteLine("📦 Inserting data into database...");
        await repository.BulkInsertTrips(uniqueTrips);
        Console.WriteLine($"✅ Successfully inserted {uniqueTrips.Count} records into the database!");

        while (true)
        {
            Console.WriteLine("============================");
            Console.WriteLine();
            Console.WriteLine("Please select an option:");
            Console.WriteLine();
            Console.WriteLine("1. 💰 Find location with highest average tip");
            Console.WriteLine("2. 🛣️ View top 100 longest trips by distance");
            Console.WriteLine("3. ⏱️ View top 100 longest trips by time");
            Console.WriteLine("4. 🔍 Search by pickup location ID");
            Console.WriteLine("5. ❌ Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (1-5): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("💰 HIGHEST AVERAGE TIP BY LOCATION 💰");
                    Console.WriteLine("====================================");
                    await DisplayHighesAverageTips();
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("🛣️ TOP 100 TRIPS BY DISTANCE 🛣️");
                    Console.WriteLine("===============================");

                    DisplayTrips(await repository.GetTop100LongestTripDistancesAsync(),
                        "Top 100 Trips by Distance (Longest First)");

                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("⏱️ TOP 100 TRIPS BY DURATION ⏱️");
                    Console.WriteLine("==============================");

                    DisplayTrips(await repository.GetTop100LongestTripTimesAsync(),
                        "Top 100 Trips by Duration (Longest First)");

                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("🔍 SEARCH BY PICKUP LOCATION 🔍");
                    Console.WriteLine("=============================");
                    Console.Write("Enter pickup location ID: ");
                    var locationId = Console.ReadLine();
                    if (int.TryParse(locationId, out int id))
                    {
                        DisplayTrips(await repository.GetTripsByPickupLocationAsync(id),
                            $"Top 100 Trips for PULocationID {id}");
                    }
                    else
                    {
                        Console.WriteLine("❌ Invalid location ID. Please enter a number.");
                    }

                    break;

                case "5":
                    Console.WriteLine("👋 Thank you!");
                    return;

                default:
                    Console.WriteLine("❌ Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task DisplayHighesAverageTips()
    {
        var location = await repository.GetLocationWithHighestAverageTipAsync();
        Console.WriteLine($"Location with the highest average tip:");
        Console.WriteLine($"PULocationID: {location.PULocationID}");
        Console.WriteLine($"Average Tip Amount: {location.AvgTipAmount:C2}");
        Console.WriteLine(new string('-', 50));
    }

    private void DisplayTrips(List<TripDto> trips, string title)
    {
        Console.WriteLine(title);
        Console.WriteLine(new string('-', 50));

        if (trips.Count != 0)
        {
            foreach (var trip in trips)
            {
                Console.WriteLine($"Trip Distance: {trip.TripDistance}");
                Console.WriteLine(
                    $"Pickup: {trip.PickupDateTime:yyyy-MM-dd HH:mm:ss}, Dropoff: {trip.DropoffDateTime:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"Fare: {trip.FareAmount:C2}, Tip: {trip.TipAmount:C2}");
                Console.WriteLine($"PULocationID: {trip.PULocationID}, DOLocationID: {trip.DOLocationID}");
                Console.WriteLine(new string('-', 50));
            }
        }
        else
        {
            Console.WriteLine("No trips found.");
        }
    }
}