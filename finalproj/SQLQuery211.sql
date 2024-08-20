alter PROCEDURE [dbo].[spUpdateCalendarEvent]
    @EventID INT,
    @UserID INT,
    @StartTime DATETIME,
    @EndTime DATETIME,
    @Name NVARCHAR(255),
    @Location NVARCHAR(255),
    @Repeat NVARCHAR(255), -- שונה ל-Repeat
    @Day INT,
    @Month INT,
    @Year INT,
    @ParentEvent INT

AS
BEGIN
    UPDATE CalendarEvents
    SET
        StartTime = @StartTime,
        EndTime = @EndTime,
        Name = @Name,
        Location = @Location,
        Repeat = @Repeat, -- שונה ל-Repeat
        Day = @Day,
        Month = @Month,
        Year = @Year,
        ParentEvent = @ParentEvent
    WHERE
        EventID = @EventID
        AND UserID = @UserID;
END