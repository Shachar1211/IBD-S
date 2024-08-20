using System;
using System.Collections.Generic;
using System.IO;

namespace finalproj.BL
{
    public class ForumQuestion
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public DateTime QuestionDateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public string Topic { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public int AnswerCount { get; set; }

        public ForumQuestion() { }

        public ForumQuestion(int userId, string title, string content, string topic, string attachment = null)
        {
            UserId = userId;
            Title = title;
            Content = content;
            Topic = topic;
            QuestionDateTime = DateTime.Now;
            Attachment = attachment;
        }

        public bool Insert()
        {
            DBservicesForumQuestion dbs = new DBservicesForumQuestion();
            int questionId = dbs.InsertQuestion(this);

            if (questionId > 0)
            {
                this.QuestionId = questionId;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles");
                Directory.CreateDirectory(path);
                if (!string.IsNullOrEmpty(Attachment))
                {
                    string filePath = $@"{path}/question_{this.QuestionId}.jpg";
                    System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(this.Attachment));
                    this.Attachment = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/question_{this.QuestionId}.jpg";

                    dbs.UpdateAttachment(this.QuestionId, this.Attachment);
                }

                return true;
            }

            return false;
        }

        public static List<ForumQuestion> ReadByTopic(string topic)
        {
            DBservicesForumQuestion dbs = new DBservicesForumQuestion();
            return dbs.ReadQuestionsByTopic(topic);
        }

        public static List<ForumQuestion> ReadAll()
        {
            DBservicesForumQuestion dbs = new DBservicesForumQuestion();
            return dbs.ReadAllQuestions();
        }

        public static ForumQuestion ReadOne(int questionId)
        {
            DBservicesForumQuestion dbs = new DBservicesForumQuestion();
            return dbs.ReadOneQuestion(questionId);
        }

        public int Update()
        {
            DBservicesForumQuestion dbs = new DBservicesForumQuestion();
            return dbs.UpdateQuestion(this);
        }

        public static int Delete(int questionId)
        {
            DBservicesForumQuestion dbs = new DBservicesForumQuestion();
            return dbs.DeleteQuestion(questionId);
        }
    }
}
