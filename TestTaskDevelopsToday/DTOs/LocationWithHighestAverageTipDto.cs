namespace TestTaskDevelopsToday.DTOs;

public record LocationWithHighestAverageTipDto(
    int PULocationID,
    decimal AvgTipAmount);