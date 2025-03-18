namespace TestTaskDevelopsToday.Data;

public static class SqlCommands
{
    public const string CreateDatabase = """
                                         IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TripsDB')
                                         BEGIN
                                             CREATE DATABASE TripsDB;
                                         END;
                                         """;


    public const string CreateTable = """
                                      USE TripsDB;

                                      IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Trips')
                                      BEGIN
                                          CREATE TABLE Trips (
                                              Id INT IDENTITY(1,1) PRIMARY KEY,
                                              PickupDateTime DATETIME NOT NULL,
                                              DropoffDateTime DATETIME NOT NULL,
                                              PassengerCount INT NOT NULL CHECK (PassengerCount >= 0),
                                              TripDistance DECIMAL(10,2) NOT NULL CHECK (TripDistance >= 0),
                                              StoreAndFwdFlag NVARCHAR(3) NOT NULL CHECK (StoreAndFwdFlag IN ('Yes', 'No')),
                                              PULocationID INT NOT NULL,
                                              DOLocationID INT NOT NULL,
                                              FareAmount DECIMAL(10,2) NOT NULL CHECK (FareAmount >= 0),
                                              TipAmount DECIMAL(10,2) NOT NULL CHECK (TipAmount >= 0)
                                          );
                                      END;
                                      """;

    public const string FindLocationWithHighestAverageTip = """
                                                            USE TripsDB;
                                                            SELECT TOP 1 PULocationID, AVG(TipAmount) AS AvgTipAmount
                                                            FROM Trips
                                                            GROUP BY PULocationID
                                                            ORDER BY AvgTipAmount DESC
                                                            """;

    public const string SelectWithLongestTripDistance = """
                                                        USE TripsDB;
                                                        SELECT TOP 100 
                                                            TripDistance, 
                                                            PickupDateTime, 
                                                            DropoffDateTime, 
                                                            FareAmount, 
                                                            TipAmount, 
                                                            PULocationID, 
                                                            DOLocationID
                                                        FROM Trips
                                                        ORDER BY TripDistance DESC;
                                                        """;

    public const string SelectWithLongestTripTime = """
                                                    USE TripsDB;
                                                    SELECT TOP 100 
                                                        TripDistance, 
                                                        PickupDateTime, 
                                                        DropoffDateTime, 
                                                        FareAmount, 
                                                        TipAmount, 
                                                        PULocationID, 
                                                        DOLocationID
                                                    FROM Trips
                                                    ORDER BY (DropoffDateTime - PickupDateTime) DESC;
                                                    """;

    public const string SearchTripsByPickUpLocation = """
                                                      USE TripsDB;
                                                      SELECT TOP 100 
                                                          TripDistance, 
                                                          PickupDateTime, 
                                                          DropoffDateTime, 
                                                          FareAmount, 
                                                          TipAmount, 
                                                          PULocationID, 
                                                          DOLocationID
                                                      FROM Trips
                                                      WHERE PULocationID = @PULocationID
                                                      ORDER BY PickupDateTime DESC;
                                                      """;
}