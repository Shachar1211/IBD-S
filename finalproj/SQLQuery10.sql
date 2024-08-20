ALTER PROCEDURE [dbo].[spReadOneCalendarEvent]
    @EventID INT,
    @UserID INT
AS
BEGIN
    SELECT 
        EventID,
        UserID,
        StartTime,
        EndTime,
        Name,
        Location,
        Repeat, -- שונה ל-Repeat
        Day,
        Month,
        Year
    FROM 
        CalendarEvents
    WHERE 
        EventID = @EventID
        AND UserID = @UserID;
END
