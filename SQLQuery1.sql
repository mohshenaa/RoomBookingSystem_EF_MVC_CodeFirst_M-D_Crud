--create database HotelDB
--go
--use HotelDB
--go
CREATE TABLE Room (
    RoomId INT IDENTITY(1000,1) PRIMARY KEY,
    RoomName VARCHAR(50) NOT NULL UNIQUE ,
    ImagePath NVARCHAR(MAX) not NULL,
    PricePerNight MONEY NOT NULL CHECK(PricePerNight > 0),
    Status BIT NOT NULL DEFAULT 1
);
 
go
CREATE TABLE BookingMaster (
    BookingId INT IDENTITY PRIMARY KEY,
    GuestName NVARCHAR(50) NOT NULL,
	Phone NVARCHAR(50) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE()
);
go
create TABLE BookingDetails (
    BookingDetailsId INT IDENTITY PRIMARY KEY,
    BookingId INT NOT NULL FOREIGN KEY REFERENCES BookingMaster(BookingId),
    RoomId INT NOT NULL FOREIGN KEY REFERENCES Room(RoomId),
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL ,
    StayingDays as datediff(day,CheckInDate,CheckOutDate) ,
    PricePerNight MONEY NOT NULL,
	Bill as (datediff(day,CheckInDate,CheckOutDate)*PricePerNight) ,
	 CONSTRAINT CHK_BookingDates CHECK (CheckOutDate > CheckInDate)
);
go
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL
);

INSERT INTO Users (Username, Password) 
VALUES ('Admin', '123456');