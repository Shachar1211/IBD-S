using System;
using System.Collections.Generic;
using System.IO;

namespace finalproj.BL
{
    public class ForumAnswer
    {
        public int UserId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public DateTime AnswerDateTime { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }

        public ForumAnswer() { }

        public ForumAnswer(int userId, int questionId, string content, string attachment = null)
        {
            UserId = userId;
            QuestionId = questionId;
            Content = content;
            AnswerDateTime = DateTime.Now;
            Attachment = attachment;
        }

        public bool Insert()
        {
            DBservicesForumAnswer dbs = new DBservicesForumAnswer();
            int answerId = dbs.InsertAnswer(this);

            if (answerId > 0)
            {
                this.AnswerId = answerId;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles");
                Directory.CreateDirectory(path);
                if (!string.IsNullOrEmpty(Attachment))
                {
                    string filePath = $@"{path}/answer_{this.AnswerId}.jpg";
                    System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(this.Attachment));
                    this.Attachment = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/answer_{this.AnswerId}.jpg";

                    dbs.UpdateAttachment(this.AnswerId, this.Attachment);
                }

                return true;
            }

            return false;
        }

        public static List<ForumAnswer> ReadByQuestion(int questionId)
        {
            DBservicesForumAnswer dbs = new DBservicesForumAnswer();
            return dbs.ReadAnswersByQuestion(questionId);
        }

        public static ForumAnswer ReadOne(int answerId)
        {
            DBservicesForumAnswer dbs = new DBservicesForumAnswer();
            return dbs.ReadOneAnswer(answerId);
        }

        public int Update()
        {
            DBservicesForumAnswer dbs = new DBservicesForumAnswer();
            return dbs.UpdateAnswer(this);
        }

        public static int Delete(int answerId)
        {
            DBservicesForumAnswer dbs = new DBservicesForumAnswer();
            return dbs.DeleteAnswer(answerId);
        }
    }
}
