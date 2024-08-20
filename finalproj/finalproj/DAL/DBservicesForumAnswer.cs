using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using finalproj.BL;
using Microsoft.Extensions.Configuration;

public class DBservicesForumAnswer
{
    public DBservicesForumAnswer() { }

    public SqlConnection connect(string conString)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString(conString);
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public int InsertAnswer(ForumAnswer answer)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = new SqlCommand("spInsertForumAnswer", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@UserId", answer.UserId);
        cmd.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
        cmd.Parameters.AddWithValue("@Content", answer.Content);
        cmd.Parameters.AddWithValue("@Attachment", answer.Attachment ?? (object)DBNull.Value);

        SqlParameter outputIdParam = new SqlParameter("@AnswerId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outputIdParam);

        cmd.ExecuteNonQuery();
        con.Close();

        return (int)outputIdParam.Value;
    }

    public void UpdateAttachment(int answerId, string attachmentUrl)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("UPDATE ForumAnswers SET Attachment = @Attachment WHERE AnswerId = @AnswerId", con)
        {
            CommandTimeout = 10,
            CommandType = CommandType.Text
        };

        cmd.Parameters.AddWithValue("@Attachment", attachmentUrl);
        cmd.Parameters.AddWithValue("@AnswerId", answerId);

        cmd.ExecuteNonQuery();
        con.Close();
    }

    public List<ForumAnswer> ReadAnswersByQuestion(int questionId)
    {
        List<ForumAnswer> answers = new List<ForumAnswer>();
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in ReadAnswersByQuestion: {ex.Message}", ex);
        }

        cmd = new SqlCommand("spReadAnswersByQuestion", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };
        cmd.Parameters.AddWithValue("@QuestionId", questionId);

        SqlDataReader dataReader = cmd.ExecuteReader();

        while (dataReader.Read())
        {
            ForumAnswer answer = new ForumAnswer
            {
                AnswerId = dataReader.GetInt32(dataReader.GetOrdinal("AnswerId")),
                QuestionId = dataReader.GetInt32(dataReader.GetOrdinal("QuestionId")),
                Content = dataReader["Content"].ToString(),
                Attachment = dataReader["Attachment"].ToString(),
                AnswerDateTime = dataReader.GetDateTime(dataReader.GetOrdinal("AnswerDateTime")),
                UserId = dataReader.GetInt32(dataReader.GetOrdinal("UserId")),
                Username = dataReader["Username"].ToString(),
                ProfilePicture = dataReader["ProfilePicture"].ToString()
            };
            answers.Add(answer);
        }

        dataReader.Close();
        con.Close();

        return answers;
    }

    public ForumAnswer ReadOneAnswer(int answerId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = new SqlCommand("spReadOneForumAnswer", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@AnswerId", answerId);

        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            ForumAnswer answer = new ForumAnswer
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                QuestionId = reader.GetInt32(reader.GetOrdinal("QuestionId")),
                AnswerId = reader.GetInt32(reader.GetOrdinal("AnswerId")),
                Content = reader["Content"].ToString(),
                Attachment = reader["Attachment"].ToString(),
                AnswerDateTime = reader.GetDateTime(reader.GetOrdinal("AnswerDateTime")),
                Username = reader["Username"].ToString(),
                ProfilePicture = reader["ProfilePicture"].ToString()
            };
            reader.Close();
            con.Close();
            return answer;
        }

        reader.Close();
        con.Close();
        return null;
    }

    public int UpdateAnswer(ForumAnswer answer)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spUpdateForumAnswer", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@AnswerId", answer.AnswerId);
        cmd.Parameters.AddWithValue("@Content", answer.Content);
        cmd.Parameters.AddWithValue("@Attachment", answer.Attachment ?? (object)DBNull.Value);

        int result = cmd.ExecuteNonQuery();
        con.Close();

        return result;
    }

    public int DeleteAnswer(int answerId)
    {
        SqlConnection con = connect("myProjDB");
        SqlCommand cmd = new SqlCommand("spDeleteForumAnswer", con)
        {
            CommandType = CommandType.StoredProcedure,
            CommandTimeout = 10
        };

        cmd.Parameters.AddWithValue("@AnswerId", answerId);

        int result = cmd.ExecuteNonQuery();
        con.Close();

        return result;
    }
}
