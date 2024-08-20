using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using finalproj.BL;
using Microsoft.Extensions.Configuration;

public class DBservicesForumQuestion
{
    public DBservicesForumQuestion() { }

    public SqlConnection connect(string conString)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString(conString);
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public int InsertQuestion(ForumQuestion question)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spInsertForumQuestion", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@UserId", question.UserId);
        cmd.Parameters.AddWithValue("@Title", question.Title);
        cmd.Parameters.AddWithValue("@Content", question.Content);
        cmd.Parameters.AddWithValue("@Attachment", question.Attachment ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Topic", question.Topic);

        SqlParameter outputIdParam = new SqlParameter("@QuestionId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outputIdParam);

        cmd.ExecuteNonQuery();
        con.Close();

        return (int)outputIdParam.Value;
    }

    public void UpdateAttachment(int questionId, string attachmentUrl)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("UPDATE ForumQuestions SET Attachment = @Attachment WHERE QuestionId = @QuestionId", con)
        {
            CommandTimeout = 10,
            CommandType = CommandType.Text
        };

        cmd.Parameters.AddWithValue("@Attachment", attachmentUrl);
        cmd.Parameters.AddWithValue("@QuestionId", questionId);

        cmd.ExecuteNonQuery();
        con.Close();
    }

    public List<ForumQuestion> ReadQuestionsByTopic(string topic)
    {
        List<ForumQuestion> questions = new List<ForumQuestion>();
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spReadQuestionsByTopic", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@Topic", topic);

        SqlDataReader dataReader = cmd.ExecuteReader();

        while (dataReader.Read())
        {
            ForumQuestion question = new ForumQuestion
            {
                QuestionId = dataReader.GetInt32(dataReader.GetOrdinal("QuestionId")),
                QuestionDateTime = dataReader.GetDateTime(dataReader.GetOrdinal("QuestionDateTime")),
                Title = dataReader["Title"].ToString(),
                Content = dataReader["Content"].ToString(),
                Attachment = dataReader["Attachment"].ToString(),
                Topic = dataReader["Topic"].ToString(),
                UserId = dataReader.GetInt32(dataReader.GetOrdinal("UserId")),
                Username = dataReader["Username"].ToString(),
                ProfilePicture = dataReader["ProfilePicture"].ToString(),
                AnswerCount = dataReader.GetInt32(dataReader.GetOrdinal("AnswerCount"))
            };
            questions.Add(question);
        }

        dataReader.Close();
        con.Close();

        return questions;
    }

    public List<ForumQuestion> ReadAllQuestions()
    {
        List<ForumQuestion> questions = new List<ForumQuestion>();
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spReadAllForumQuestions", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        SqlDataReader dataReader = cmd.ExecuteReader();

        while (dataReader.Read())
        {
            ForumQuestion question = new ForumQuestion
            {
                QuestionId = dataReader.GetInt32(dataReader.GetOrdinal("QuestionId")),
                QuestionDateTime = dataReader.GetDateTime(dataReader.GetOrdinal("QuestionDateTime")),
                Title = dataReader["Title"].ToString(),
                Content = dataReader["Content"].ToString(),
                Attachment = dataReader["Attachment"].ToString(),
                Topic = dataReader["Topic"].ToString(),
                UserId = dataReader.GetInt32(dataReader.GetOrdinal("UserId")),
                Username = dataReader["Username"].ToString(),
                ProfilePicture = dataReader["ProfilePicture"].ToString(),
                AnswerCount = dataReader.GetInt32(dataReader.GetOrdinal("AnswerCount"))
            };
            questions.Add(question);
        }

        dataReader.Close();
        con.Close();

        return questions;
    }

    public ForumQuestion ReadOneQuestion(int questionId)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spReadOneForumQuestion", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@QuestionId", questionId);

        SqlDataReader dataReader = cmd.ExecuteReader();

        if (dataReader.Read())
        {
            ForumQuestion question = new ForumQuestion
            {
                QuestionId = dataReader.GetInt32(dataReader.GetOrdinal("QuestionId")),
                QuestionDateTime = dataReader.GetDateTime(dataReader.GetOrdinal("QuestionDateTime")),
                Title = dataReader["Title"].ToString(),
                Content = dataReader["Content"].ToString(),
                Attachment = dataReader["Attachment"].ToString(),
                Topic = dataReader["Topic"].ToString(),
                UserId = dataReader.GetInt32(dataReader.GetOrdinal("UserId")),
                Username = dataReader["Username"].ToString(),
                ProfilePicture = dataReader["ProfilePicture"].ToString(),
                AnswerCount = dataReader.GetInt32(dataReader.GetOrdinal("AnswerCount"))
            };
            dataReader.Close();
            con.Close();
            return question;
        }

        dataReader.Close();
        con.Close();
        return null;
    }

    public int UpdateQuestion(ForumQuestion question)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spUpdateForumQuestion", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@QuestionId", question.QuestionId);
        cmd.Parameters.AddWithValue("@Title", question.Title);
        cmd.Parameters.AddWithValue("@Content", question.Content);
        cmd.Parameters.AddWithValue("@Attachment", question.Attachment ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Topic", question.Topic);

        int result = cmd.ExecuteNonQuery();
        con.Close();

        return result;
    }

    public int DeleteQuestion(int questionId)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spDeleteForumQuestion", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@QuestionId", questionId);

        int result = cmd.ExecuteNonQuery();
        con.Close();

        return result;
    }
}
