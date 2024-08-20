drop table files

drop table Documents

CREATE TABLE [dbo].[Files] (
    [FilesId]  INT            IDENTITY (1, 1) NOT NULL,
    [UserId]   INT            NOT NULL,
    [FileName] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([FilesId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]) on delete cascade
);


CREATE TABLE [dbo].[Documents] (
    [DocumentId]   INT            IDENTITY (1, 1) NOT NULL,
    [FileId]       INT            NULL,
    [UserId]       INT            NOT NULL,
    [DocumentName] NVARCHAR (255) NOT NULL,
    [DocumentPath] NVARCHAR (255) NOT NULL,
    [UploadDate]   DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([DocumentId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserID]),
    FOREIGN KEY ([FileId]) REFERENCES [dbo].[Files] ([FilesId]) on delete cascade
);

