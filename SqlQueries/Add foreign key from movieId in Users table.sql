﻿
ALTER TABLE dbo.Users 
ADD MovieId INT;

ALTER TABLE dbo.Users 
ADD FOREIGN KEY (MovieId) REFERENCES dbo.Movies(Id);

ALTER TABLE dbo.Users 
ADD UserId INT;

ALTER TABLE dbo.Movies 
ADD FOREIGN KEY (UserId) REFERENCES dbo.Movies(UserId);

SELECT username, email FROM dbo.Users
JOIN Movies as m ON MovieId = m.ID
WHERE MovieId = 5
