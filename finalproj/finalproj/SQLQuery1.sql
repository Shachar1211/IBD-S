CREATE TABLE [dbo].[Users] (
    [UserID]         INT            IDENTITY (1, 1) NOT NULL,
    [Username]       NVARCHAR (50)  NOT NULL,
    [Email]          NVARCHAR (255) NOT NULL,
    [Password]       NVARCHAR (255) NOT NULL,
    [FirstName]      NVARCHAR (50)  NOT NULL,
    [LastName]       NVARCHAR (50)  NOT NULL,
    [DateOfBirth]    DATE           NOT NULL,
    [Gender]         NVARCHAR (10)  NOT NULL,
    [CreationDate]   DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [TypeOfIBD]      NVARCHAR (50)  NOT NULL,
    [ProfilePicture] NVARCHAR (255) ,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC),
    CHECK ([TypeOfIBD]='אחר' OR [TypeOfIBD]='קוליטיס' OR [TypeOfIBD]='קרוהן')
);

