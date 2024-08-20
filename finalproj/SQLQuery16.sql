ALTER PROCEDURE spInsertAlert
    @EventID INT,
    @Aname NVARCHAR(50),
    @Arepeat NVARCHAR(50),
    @AlertTime DATETIME
AS
BEGIN
    INSERT INTO Alerts (EventID, Aname, Arepeat, AlertTime)
    VALUES (@EventID, @Aname, @Arepeat, @AlertTime)
END
