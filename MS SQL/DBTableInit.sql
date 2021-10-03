use master
go
create database Contoso_Authentication_Logs
go
use Contoso_Authentication_Logs
go
create table Months
(
    MonthID tinyint primary key identity,
    MonthName nvarchar(20)
)
go
create table DeviceTypes
(
    DeviceID tinyint primary key identity,
    DeviceName nvarchar(20)
)
go
create table RegistrationCountByMonth
(
    RegistrationCountByMonthID int primary key identity,
    Year smallint,
    Month tinyint,
    NumberOfUsers int
)
go
alter table RegistrationCountByMonth
    add foreign key (Month) references Months(MonthID)
go
create table RegistrationCountByDevicesAndMonth
(
    RegistrationCountByDevicesAndMonthID int primary key identity,
    Year smallint,
    Month tinyint,
    DeviceType tinyint,
    NumberOfUsers int
)
go
alter table RegistrationCountByDevicesAndMonth
    add foreign key (Month) references Months(MonthID),
        foreign key (DeviceType) references DeviceTypes(DeviceID)
go
create table ConcurrentSessionsEveryHour
(
    ConcurrentSessionsEveryHourID int primary key identity,
    Hour datetime unique,
    NumberOfUsers int
)
go
create table ConcurrentUniqueSessionsWithMultipleDevices
(
    ConcurrentUniqueSessionsWithMultipleDevicesID int primary key identity,
    UserName nvarchar(50),
    DeviceName nvarchar(50),
    LoginTS datetime
)
go
create table TotalSessionDurationByHour
(
    TotalSessionDurationByHourID int primary key identity,
    Date date,
    Hour tinyint,
    TotalSessionDurationForHourInMins int,
    TotalSessionDuration int
)
go
create table UniqueCountriesByDay
(
    UniqueCountriesByDayID int primary key identity,
    UserName nvarchar(50),
    Country nvarchar(30),
    LoginTS datetime
)
go