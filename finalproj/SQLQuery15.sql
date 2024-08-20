
ALTER PROCEDURE [dbo].[spInsertAlert]
    @EventID INT,
    @Aname NVARCHAR(255),
    @Arepeat NVARCHAR(255),
    @AlertTime DATETIME
AS
BEGIN
    INSERT INTO Alerts (
        EventID,
        Aname,
        Arepeat,
        AlertTime
    ) VALUES (
        @EventID,
        @Aname,
        @Arepeat,
        @AlertTime
    );
END
