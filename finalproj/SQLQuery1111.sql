drop table [ForumAnswers]
drop table ForumQuestions

CREATE TABLE [dbo].[ForumQuestions] (
    [UserId]           INT             NULL,
    [QuestionId]       INT            IDENTITY (1, 1) NOT NULL,
    [QuestionDateTime] DATETIME2 (7)  DEFAULT (getdate()) NULL,
    [Title]            NVARCHAR (255) NOT NULL,
    [Content]          NVARCHAR (MAX) NOT NULL,
    [Attachment]       NVARCHAR (255) NULL,
    [Topic]            NVARCHAR (255) NOT NULL,
    [AnswerCount]      INT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE cascade
);

CREATE TABLE [dbo].[ForumAnswers] (
    [AnswerId]       INT            IDENTITY (1, 1) NOT NULL,
    [QuestionId]     INT            NOT NULL,
    [UserId]         INT            NULL,
    [Content]        NVARCHAR (MAX) NOT NULL,
    [Attachment]     NVARCHAR (255) NULL,
    [AnswerDateTime] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([AnswerId] ASC),
    FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[ForumQuestions] ([QuestionId]) ON DELETE CASCADE,
);
