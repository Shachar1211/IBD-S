CREATE OR ALTER PROCEDURE spDeleteUser
    @Email VARCHAR(255),
    @EffectedRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the user exists
    DECLARE @UserId INT;
    SELECT @UserId = UserID FROM Users WHERE Email = @Email;

    IF @UserId IS NULL
    BEGIN
        PRINT 'User not found.';
        SET @EffectedRows = -1;
        RETURN;
    END

    -- Begin transaction
    BEGIN TRANSACTION;

    -- Initialize the row count
    DECLARE @TotalDeleted INT;
    SET @TotalDeleted = 0;

    -- Delete from Friends table first
    DELETE FROM [dbo].[Friends]
    WHERE UserId = @UserId
       OR FriendId = @UserId;

    -- Add the number of rows affected by the Friends table deletion to the total count
    SET @TotalDeleted = @TotalDeleted + @@ROWCOUNT;

    delete from dbo.[Documents]
    where userId = @UserId
    SET @TotalDeleted = @TotalDeleted + @@ROWCOUNT;

    delete from [dbo].[Chat]
    where RecipientId = @UserId or senderId = @UserId;

   
    -- Then delete from Users table
    DELETE FROM Users
    WHERE UserID = @UserId;

    -- Add the number of rows affected by the Users table deletion to the total count
    SET @TotalDeleted = @TotalDeleted + @@ROWCOUNT;

    -- Commit transaction
    COMMIT;

    -- Set output parameter to the number of rows affected
    SET @EffectedRows = @TotalDeleted;
END;
