CREATE PROCEDURE InsertMail
    @UserId INT,
    @SenderUserId INT = NULL,
    @Picture NVARCHAR(MAX) = NULL,
    @Username NVARCHAR(50) = NULL,
    @SendDate DATETIME = NULL,
    @ForumQuestionId INT = NULL,
    @ForumSubject NVARCHAR(100) = NULL,
    @ForumContent NVARCHAR(MAX) = NULL,
    @CalendarEventId INT = NULL,
    @CalendarEventName NVARCHAR(100) = NULL,
    @CalendarEventLocation NVARCHAR(100) = NULL,
    @MailFromCalendar BIT
AS
BEGIN
    INSERT INTO Mail (UserId, SenderUserId, Picture, Username, SendDate, ForumQuestionId, ForumSubject, ForumContent, CalendarEventId, CalendarEventName, CalendarEventLocation, MailFromCalendar)
    VALUES (@UserId, @SenderUserId, @Picture, @Username, @SendDate, @ForumQuestionId, @ForumSubject, @ForumContent, @CalendarEventId, @CalendarEventName, @CalendarEventLocation, @MailFromCalendar);
END;