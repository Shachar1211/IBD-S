CREATE or alter PROCEDURE GetMailsByUserId
    @UserId INT
AS
BEGIN
    SELECT 
        MailId,
        UserId,
        SenderUserId,
        Picture,
        Username,
        SendDate,
        ForumQuestionId,
        ForumSubject,
        ForumContent,
        CalendarEventId,
        CalendarEventName,
        CalendarEventLocation,
		CalenderEventStartTime,
        MailFromCalendar
    FROM 
        Mail
    WHERE 
        UserId = @UserId;
END;