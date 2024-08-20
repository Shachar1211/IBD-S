using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using finalproj.BL;

namespace finalproj.DAL
{
    public class DBservicesMail
    {
        public SqlConnection connect(string conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        public bool Insert(Mail mail)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = CreateMailInsertCommandWithStoredProcedure("InsertMail", con, mail); // יצירת הפקודה

                int numEffected = cmd.ExecuteNonQuery(); // ביצוע הפקודה
                return numEffected > 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // סגירת החיבור למסד הנתונים
                    con.Close();
                }
            }
        }
        private SqlCommand CreateMailInsertCommandWithStoredProcedure(string spName, SqlConnection con, Mail mail)
        {
            SqlCommand cmd = new SqlCommand(spName, con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה

            cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
            cmd.CommandTimeout = 10;

            // הוספת פרמטרים עם ערכים לאובייקט הפקודה
            cmd.Parameters.AddWithValue("@UserId", mail.Userid);
            cmd.Parameters.AddWithValue("@SenderUserId", mail.SenderUserId);
            cmd.Parameters.AddWithValue("@Picture", mail.Picture);
            cmd.Parameters.AddWithValue("@Username", mail.Username);
           // cmd.Parameters.AddWithValue("@SendDate", mail.SendDate);
            cmd.Parameters.AddWithValue("@ForumQuestionId", mail.ForumQustionId);
            cmd.Parameters.AddWithValue("@ForumSubject",mail.ForumSubject);
            cmd.Parameters.AddWithValue("@ForumContent", mail.ForumContent);
            cmd.Parameters.AddWithValue("@CalendarEventId", mail.CalendarEventId);
            cmd.Parameters.AddWithValue("@CalendarEventName", mail.CalendaerEventName);
            cmd.Parameters.AddWithValue("@CalendarEventLocation",mail.CalendarEventLocation);
            cmd.Parameters.AddWithValue("@CalenderEventStartTime", mail.CalenderEventStartTime);
            cmd.Parameters.AddWithValue("@MailFromCalendar", mail.MailFromCalander);

            return cmd;
        }



        public List<Mail> GetMailsByUserId(int userId)
        {
            List<Mail> mails = new List<Mail>();
            SqlConnection con = null;
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = new SqlCommand("GetMailsByUserId", con); // יצירת הפקודה
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Mail mail = new Mail
                    {
                        Mailid = (int)reader["MailId"],
                        Userid = (int)reader["UserId"],
                        Picture = reader["Picture"].ToString(),
                        Username = reader["Username"].ToString(),
                        SenderUserId = (int)reader["SenderUserId"],
                        SendDate = (DateTime)reader["SendDate"],
                        ForumQustionId = (int)reader["ForumQuestionId"],
                        ForumSubject = reader["ForumSubject"].ToString(),
                        ForumContent = reader["ForumContent"].ToString(),
                        CalendarEventId = (int)reader["CalendarEventId"],
                        CalendaerEventName = reader["CalendarEventName"].ToString(),
                        CalendarEventLocation = reader["CalendarEventLocation"].ToString(),
                        CalenderEventStartTime = reader["CalenderEventStartTime"].ToString(),
                        MailFromCalander = (bool)reader["MailFromCalendar"]
                    };
                    mails.Add(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }

            return mails;
        }


        public bool DeleteByQuestion(int forumQuestionId)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in Delete: {ex.Message}", ex);
            }

            cmd = new SqlCommand("DeleteMailByForumQuestionId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ForumQuestionId", forumQuestionId);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in Delete: {ex.Message}", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public bool DeleteByCalender(int calenderID)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in Delete: {ex.Message}", ex);
            }

            cmd = new SqlCommand("DeleteMailBycalendarEventId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@calendarEventId", calenderID);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in Delete: {ex.Message}", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }



    }
}
