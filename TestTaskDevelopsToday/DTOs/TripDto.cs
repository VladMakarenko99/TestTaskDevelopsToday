namespace TestTaskDevelopsToday.DTOs;

public record TripDto(
    decimal TripDistance,
    DateTime PickupDateTime,
    DateTime DropoffDateTime,
    decimal FareAmount,
    decimal TipAmount,
    int PULocationID,
    int DOLocationID
);