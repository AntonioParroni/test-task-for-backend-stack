use testDB
go
INSERT Months VALUES
('January')
,('February')
,('March')
,('April')
,('May')
,('June')
,('July')
,('August')
,('September')
,('October')
,('November')
,('December')
go
INSERT DeviceTypes VALUES
('Laptop')
,('Mobile Phone')
,('Tablet')
go
INSERT RegistrationCountByMonth VALUES
(2020, 8, 13),
(2020, 9, 5),
(2020, 10, 7),
(2021, 1, 9),
(2021, 2, 6),
(2021, 7, 32)
go
INSERT RegistrationCountByDevicesAndMonth VALUES
(2020, 8, 1, 10),
(2020, 8, 2, 3),
(2020, 9, 1, 5),
(2021, 7, 1, 15),
(2021, 7, 2, 8),
(2021, 7, 3, 9)
go
INSERT ConcurrentSessionsEveryHour VALUES
('2021-07-01 13:00:00',3),
('2021-07-01 14:00:00',23),
('2021-07-01 15:00:00',19),
('2021-07-01 16:00:00',10),
('2021-07-01 17:00:00',15),
('2021-07-01 18:00:00',8)
go
INSERT ConcurrentUniqueSessionsWithMultipleDevices VALUES
('John Doe','John''s Laptop','2021-07-01 17:35:18'),
('John Doe','John''s Mobile phone','2021-07-01 17:55:47'),
('Kathy Johnson','My Macbook','2021-07-01 18:11:23'),
('Kathy Johnson','My IPhone','2021-07-01 18:13:26'),
('Kathy Johnson','My IPad','2021-07-01 18:29:31')
go
INSERT TotalSessionDurationByHour VALUES
('2021-06-29', 0, 500, 500),
('2021-06-29', 1, 342, 842),
('2021-06-29', 2, 100, 942),
('2021-06-29', 23, 154, 15643),
('2021-06-30', 0, 100, 100),
('2021-06-30', 1, 200, 300),
('2021-07-01', 0, 450, 450),
('2021-07-01', 1, 200, 650),
('2021-07-01', 18, 100, 21350)
go
INSERT UniqueCountriesByDay VALUES
('John Doe','Switzerland','2021-07-01 17:35:18'),
('Kathy Johnson','Turkey','2021-07-01 18:11:23')