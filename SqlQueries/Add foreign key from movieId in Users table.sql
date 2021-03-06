﻿
ALTER TABLE dbo.Users 
ADD MovieId INT;

ALTER TABLE dbo.Users 
DROP MovieId;

ALTER TABLE dbo.Users 
ADD FOREIGN KEY (MovieId) REFERENCES dbo.Movies(Id);

ALTER TABLE dbo.Users 
ADD UserId INT;

ALTER TABLE dbo.Movies 
ADD FOREIGN KEY (UserId) REFERENCES dbo.Movies(UserId);

SELECT username, email FROM dbo.Users
JOIN Movies as m ON MovieId = m.ID
WHERE MovieId = 5


CREATE TABLE Tokens (
TokenValue VARCHAR(20) PRIMARY KEY,
UserId INT
);

CREATE TABLE Tickets (
Id int identity PRIMARY KEY,
Ticket_Date datetime,
Ticket_MovieId int,
Ticket_UserId int
);

ALTER TABLE Tickets
ADD FOREIGN KEY (Ticket_UserId) REFERENCES Users(Id)

ALTER TABLE Tickets
ADD FOREIGN KEY (Ticket_MovieId) REFERENCES Movies(Id)


ALTER TABLE Tokens 
ADD FOREIGN KEY (UserId) REFERENCES Users(Id)

Create Table Users
(
	Id int identity primary key,
	Username nvarchar(100),
	Password nvarchar(100)
 )

DELETE FROM Tokens;

 Insert into Users values ('Alex','123')
 Insert into Users values ('Diana','321')
 Insert into Users values ('John','453')
Insert into Users values ('Martin','1234')
 Insert into Users values ('Allan','3214')
 Insert into Users values ('Pierre','4534')
