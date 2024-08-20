

CREATE or alter PROCEDURE spReadQuestionsByTopic
    @Topic NVARCHAR(255)
AS
BEGIN
    SELECT q.QuestionId, q.QuestionDateTime, q.Title, q.Content, q.Attachment, q.Topic, q.UserId, u.Username, u.ProfilePicture,
           (SELECT COUNT(*) FROM ForumAnswers WHERE QuestionId = q.QuestionId) AS AnswerCount
    FROM ForumQuestions q
    JOIN Users u ON q.UserId = u.UserID
    WHERE q.Topic = @Topic
    order by q.QuestionId desc
END;