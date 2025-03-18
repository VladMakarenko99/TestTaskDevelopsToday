using TestTaskDevelopsToday.Entities;

namespace TestTaskDevelopsToday.Utilities;

public class DuplicateCleanerUtility
{
    public static (List<Trip> uniqueRecords, List<Trip> duplicates) RemoveDuplicates(List<Trip> trips)
    {
        var grouped = trips.GroupBy(t => new { t.PickupDateTime, t.DropoffDateTime, t.PassengerCount });

        var uniqueTrips = new List<Trip>();
        var duplicateTrips = new List<Trip>();

        foreach (var group in grouped)
        {
            var tripList = group.ToList();
            uniqueTrips.Add(tripList.First());

            if (tripList.Count > 1)
            {
                duplicateTrips.AddRange(tripList.Skip(1));
            }
        }

        return (uniqueTrips, duplicateTrips);
    }
}