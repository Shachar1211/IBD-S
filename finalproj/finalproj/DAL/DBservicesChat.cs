using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using finalproj.BL;

namespace finalproj.DAL
{
    public class DBservicesChat
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

        public bool Insert(Chat chat)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = CreateChatInsertCommandWithStoredProcedure("spInsertChat", con, chat); // יצירת הפקודה

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

        private SqlCommand CreateChatInsertCommandWithStoredProcedure(string spName, SqlConnection con, Chat chat)
        {
            SqlCommand cmd = new SqlCommand(spName, con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה

            cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
            cmd.CommandTimeout = 10;

            // הוספת פרמטרים עם ערכים לאובייקט הפקודה
            cmd.Parameters.AddWithValue("@SenderId", chat.SenderId);
            cmd.Parameters.AddWithValue("@RecipientId", chat.RecipientId);
            cmd.Parameters.AddWithValue("@Contenct", chat.Contenct); // טיפול בערך null אפשרי
            cmd.Parameters.AddWithValue("@SendDate", chat.SendDate);
            cmd.Parameters.AddWithValue("@AttachedFile", chat.AttachedFile);

            return cmd;
        }

        public List<Chat> GetFullChat(int user1Id, int user2Id)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Chat> chats = new List<Chat>();

            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = new SqlCommand("spGetChatWithFriendStatus", con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה
                cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
                cmd.CommandTimeout = 10;

                // הוספת פרמטרים עם ערכים לאובייקט הפקודה
                cmd.Parameters.AddWithValue("@User1Id", user1Id);
                cmd.Parameters.AddWithValue("@User2Id", user2Id);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Chat chat = new Chat
                    {
                        ChatId = (int)reader["ChatId"],
                        SenderId = (int)reader["SenderId"],
                        RecipientId = (int)reader["RecipientId"],
                        Contenct = reader["Contenct"].ToString(),
                        SendDate = (DateTime)reader["SendDate"],
                        AttachedFile = (bool)reader["AttachedFile"],
                        //User2Username = reader["User2Username"].ToString(),
                        //User2ProfilePicture = reader["User2ProfilePicture"].ToString(),
                        AreFriends = (bool)reader["AreFriends"]
                    };
                    chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }

            return chats;
        }
        public List<Chat> GetLatestMessages(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Chat> chats = new List<Chat>();

            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = new SqlCommand("spGetLatestMessages", con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה
                cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
                cmd.CommandTimeout = 10;

                // הוספת פרמטרים עם ערכים לאובייקט הפקודה
                cmd.Parameters.AddWithValue("@UserId", userId);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Chat chat = new Chat
                    {
                        ChatId = (int)reader["ChatId"],
                        SenderId = (int)reader["SenderId"],
                        RecipientId = (int)reader["RecipientId"],
                        Contenct = reader["Contenct"].ToString(),
                        SendDate = (DateTime)reader["SendDate"],
                        AttachedFile = (bool)reader["AttachedFile"],
                        User2Username = reader["User2Username"].ToString(),
                        User2ProfilePicture = reader["User2ProfilePicture"].ToString()
                    };
                    chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }

            return chats;
        }


        public List<Chat> GetFullChatFromDate(int user1Id, int user2Id, DateTime startDate)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Chat> chats = new List<Chat>();

            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = new SqlCommand("spGetChatWithFriendStatusFromDate", con); // יצירת אובייקט הפקודה והקצאת שם הפרוצדורה השמורה
                cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
                cmd.CommandTimeout = 10;

                // הוספת פרמטרים עם ערכים לאובייקט הפקודה
                cmd.Parameters.AddWithValue("@User1Id", user1Id);
                cmd.Parameters.AddWithValue("@User2Id", user2Id);
                cmd.Parameters.AddWithValue("@StartDate", startDate);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Chat chat = new Chat
                    {
                        ChatId = (int)reader["ChatId"],
                        SenderId = (int)reader["SenderId"],
                        RecipientId = (int)reader["RecipientId"],
                        Contenct = reader["Contenct"].ToString(),
                        SendDate = (DateTime)reader["SendDate"],
                        AttachedFile = (bool)reader["AttachedFile"],
                        AreFriends = (bool)reader["AreFriends"]
                    };
                    chats.Add(chat);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // סגירת החיבור למסד הנתונים
                }
            }

            return chats;
        }


    }


}
