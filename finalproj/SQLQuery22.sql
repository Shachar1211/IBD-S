CREATE TABLE Articles (
    InfoId INT IDENTITY (1, 1) NOT NULL ,
    Picture NVARCHAR(255),
    Header NVARCHAR(255),
    Contenct NVARCHAR(MAX),
    Link NVARCHAR(255)
    PRIMARY KEY CLUSTERED (InfoId ASC),
);