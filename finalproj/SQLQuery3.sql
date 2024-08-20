CREATE OR ALTER PROCEDURE spGetLatestMessages
    @UserId INT
AS
BEGIN
    WITH LatestChats AS (
        SELECT
            ChatId,
            SenderId,
            RecipientId,
            Contenct,
            SendDate,
            AttachedFile,
            CASE 
                WHEN SenderId = @UserId THEN RecipientId
                ELSE SenderId 
            END AS User2Id,
            ROW_NUMBER() OVER (PARTITION BY CASE 
                                              WHEN SenderId = @UserId THEN RecipientId
                                              ELSE SenderId 
                                           END
                               ORDER BY SendDate DESC, ChatId DESC) AS rn
        FROM 
            Chat
        WHERE 
            SenderId = @UserId OR RecipientId = @UserId
    )
    SELECT 
        lc.ChatId,
        lc.SenderId,
        lc.RecipientId,
        lc.Contenct,
        lc.SendDate,
        lc.AttachedFile,
        u.UserID AS User2Id,
        u.Username AS User2Username,
        u.ProfilePicture AS User2ProfilePicture
    FROM
        LatestChats lc
    INNER JOIN 
        Users u ON lc.User2Id = u.UserID
    WHERE
        lc.rn = 1
    ORDER BY
        lc.SendDate DESC;
END;


exec spGetLatestMessages 15






