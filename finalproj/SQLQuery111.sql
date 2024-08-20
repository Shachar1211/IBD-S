drop table Friends


CREATE TABLE [dbo].[Friends] (
    [UserId]   INT NOT NULL,
    [FriendId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [FriendId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]),
    FOREIGN KEY ([FriendId]) REFERENCES [dbo].[Users] ([UserID])
);

