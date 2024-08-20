CREATE TABLE [dbo].[Chat] (
    ChatId INT IDENTITY (1, 1) NOT NULL,
    SenderId INT,
    RecipientId INT,
    Contenct VARCHAR(255),
    SendDate DATETIME,
    AttachedFile bit,
    PRIMARY KEY CLUSTERED ([ChatId] ASC),
    FOREIGN KEY (SenderId) REFERENCES Users(UserId),
    FOREIGN KEY (RecipientId) REFERENCES Users(UserId)
);


