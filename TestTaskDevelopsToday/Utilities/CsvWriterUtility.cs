using System.Globalization;
using CsvHelper;
using TestTaskDevelopsToday.Entities;

namespace TestTaskDevelopsToday.Utilities;

public static class CsvWriterUtility
{
    public static async Task WriteToCsv(List<Trip> records, string filePath)
    {
        await using var writer = new StreamWriter(filePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
    }
}