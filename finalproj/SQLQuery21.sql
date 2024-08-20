CREATE PROCEDURE spReadFilesForUser
    @UserId NVARCHAR(255)
AS
BEGIN
    SELECT 
        [FilesId],
        [UserId] ,
        [FileName],
        [FilePath]
    FROM Files
    WHERE UserId =  @UserId
END;

