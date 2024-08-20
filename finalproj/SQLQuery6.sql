CREATE or alter PROCEDURE spReadForumQuestionsByTopic
    @Topic NVARCHAR(255)
AS
BEGIN
    SELECT UserId, QuestionId, QuestionDateTime, Title, Content, Attachment, Topic
    FROM ForumQuestions
    WHERE Topic = @Topic
    order by QuestionId desc
END