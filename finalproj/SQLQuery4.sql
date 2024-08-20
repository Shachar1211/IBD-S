drop table mail

CREATE TABLE [dbo].[Mail] (
    [MailId]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserId]                 INT            NOT NULL,
    [Picture]                NVARCHAR (MAX) NULL,
    [SenderUserId]           INT            NULL,
    [Username]               NVARCHAR (50)  NULL,
    [SendDate]               DATETIME       NULL,
    [ForumQuestionId]        INT            NULL,
    [ForumSubject]           NVARCHAR (100) NULL,
    [ForumContent]           NVARCHAR (MAX) NULL,
    [CalendarEventId]        INT            NULL,
    [CalendarEventName]      NVARCHAR (100) NULL,
    [CalendarEventLocation]  NVARCHAR (100) NULL,
    [calenderEventStartTime] NVARCHAR (25)  NULL,
    [MailFromCalendar]       BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([MailId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]) on delete cascade
);

