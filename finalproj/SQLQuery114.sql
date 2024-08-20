ALTER PROCEDURE [dbo].[spInsertAlert]
    @EventID INT,
    @Aname NVARCHAR(255),
    @Arepeat NVARCHAR(255)
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
        DATEADD(MINUTE, -CAST(@Arepeat AS INT), (SELECT StartTime FROM CalendarEvents WHERE EventID = @EventID))
    );
END
