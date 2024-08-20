
CREATE PROCEDURE [dbo].[spGetChatWithFriendStatusFromDate]
    @User1Id INT,
    @User2Id INT,
    @StartDate DATETIME
AS
BEGIN
    SELECT 
        c.ChatId,
        c.SenderId,
        c.RecipientId,
        c.Contenct,
        c.SendDate,
        c.AttachedFile,
        CASE
            WHEN EXISTS (SELECT 1 FROM Friends f WHERE (f.UserId = @User1Id AND f.FriendId = @User2Id))
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT)
        END AS AreFriends
    FROM 
        Chat c
    WHERE 
        ((c.SenderId = @User1Id AND c.RecipientId = @User2Id)
        OR 
        (c.SenderId = @User2Id AND c.RecipientId = @User1Id))
        AND c.SendDate >= @StartDate
    ORDER BY 
        c.SendDate ASC;
END
