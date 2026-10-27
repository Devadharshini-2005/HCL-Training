Create Database FlightBookingDB;
go

Use FlightBookingDB;
Go

-- Flight table
CREATE TABLE Flights
(
    FlightId INT PRIMARY KEY IDENTITY(1,1),
    FlightName NVARCHAR(100) NOT NULL,
    Source NVARCHAR(100) NOT NULL,
    Destination NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0)
);
GO

-- Booking table
CREATE TABLE Bookings
(
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    PassengerName NVARCHAR(100) NOT NULL,
    FlightId INT NOT NULL,
    Seats INT NOT NULL CHECK (Seats > 0),
    TotalPrice DECIMAL(12,2) NOT NULL,

    CONSTRAINT FK_Bookings_Flights
        FOREIGN KEY (FlightId)
        REFERENCES Flights(FlightId)
);
GO

-- Insert sample flights
INSERT INTO Flights
    (FlightName, Source, Destination, Price)
VALUES
    ('Air India', 'Chennai', 'Delhi', 4500.00),
    ('IndiGo', 'Madurai', 'Bangalore', 3200.00),
    ('SpiceJet', 'Chennai', 'Mumbai', 5000.00);
GO

-- Verify the tables
SELECT * FROM Flights;
SELECT * FROM Bookings;
GO


