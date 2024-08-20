using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using finalproj.BL;
using Microsoft.Extensions.Configuration;

public class DBservicesUser
{
    public DBservicesUser() { }

    public SqlConnection connect(String conString)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString(conString);
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public int Insert(User user)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB"); // יצירת החיבור
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateUserInsertCommandWithStoredProcedure("spInsertUser", con, user); // יצירת הפקודה

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // ביצוע הפקודה
            return numEffected;
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

    private SqlCommand CreateUserInsertCommandWithStoredProcedure(string spName, SqlConnection con, User user)
    {
        SqlCommand cmd = new SqlCommand(spName, con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה

        cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
        cmd.CommandTimeout = 10;

        // הוספת פרמטרים עם ערכים לאובייקט הפקודה
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
        cmd.Parameters.AddWithValue("@LastName", user.LastName);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Password", user.Password);
        cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
        cmd.Parameters.AddWithValue("@Gender", user.Gender);
        cmd.Parameters.AddWithValue("@TypeOfIBD", user.TypeOfIBD);
        cmd.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture ?? (object)DBNull.Value); // טיפול בערך null אפשרי

        return cmd;
    }

    public int Update(User user)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // יצירת החיבור
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateUpdateCommandWithStoredProcedure("spUpdateUser", con, user); // יצירת הפקודה

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // ביצוע הפקודה
            return numEffected;
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

    private SqlCommand CreateUpdateCommandWithStoredProcedure(string spName, SqlConnection con, User user)
    {
        SqlCommand cmd = new SqlCommand(spName, con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה

        cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
        cmd.CommandTimeout = 10;

        // הוספת פרמטרים עם ערכים לאובייקט הפקודה, בהתאמה לסכמת מסד הנתונים
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
        cmd.Parameters.AddWithValue("@LastName", user.LastName);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Password", user.Password);
        cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
        cmd.Parameters.AddWithValue("@Gender", user.Gender);
        cmd.Parameters.AddWithValue("@TypeOfIBD", user.TypeOfIBD);
        cmd.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture ?? (object)DBNull.Value); // טיפול בערך null אפשרי לשדות אופציונליים
        return cmd;
    }

    public List<User> Read()
    {
        List<User> users = new List<User>();
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // יצירת החיבור
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in Read: {ex.Message}", ex);
        }

        cmd = new SqlCommand("spReadUser", con); // יצירת הפקודה
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(); // ביצוע הפקודה

            while (dataReader.Read())
            {
                User user = new User
                {
                    Id = dataReader.GetInt32(0), // קבלת המזהה מהעמודה הראשונה
                    Username = dataReader["Username"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    Email = dataReader["Email"].ToString(),
                    Password = dataReader["Password"].ToString(),
                    DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                    Gender = dataReader["Gender"].ToString(),
                    TypeOfIBD = dataReader["TypeOfIBD"].ToString(),
                    ProfilePicture = dataReader["ProfilePicture"] != DBNull.Value ? dataReader["ProfilePicture"].ToString() : null
                };
                users.Add(user);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in Read: {ex.Message}", ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

        return users;
    }

    public User LogIn(User user)
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

        cmd = CreateLogInCommandWithStoredProcedureWithoutParameters("spLogInUser", con, user); // יצירת הפקודה

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                User loggedInUser = new User
                {
                    Id = dataReader.GetInt32(0),
                    Username = dataReader["Username"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    Email = dataReader["Email"].ToString(),
                    Password = dataReader["Password"].ToString(),
                    DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                    Gender = dataReader["Gender"].ToString(),
                    TypeOfIBD = dataReader["TypeOfIBD"].ToString(),
                    ProfilePicture = dataReader["ProfilePicture"].ToString()
                };
                return loggedInUser;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateLogInCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con, User user)
    {
        SqlCommand cmd = new SqlCommand(spName, con)
        {
            CommandTimeout = 10,
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);

        return cmd;
    }

    public bool Delete(User user)
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

        cmd = new SqlCommand("spDeleteUser", con)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Email", user.Email);

        // Add output parameter
        SqlParameter outputParam = new SqlParameter("@EffectedRows", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outputParam);

        try
        {
            cmd.ExecuteNonQuery();

            // Get the value of the output parameter
            int effectedRows = (int)outputParam.Value;

            return effectedRows > 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting user: " + ex.Message);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public User ReadOne(string email)
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

        cmd = CreateReadOneCommandWithStoredProcedure("spReadOneUser", con, email); // יצירת הפקודה

        try
        {
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                User user = new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserID")),
                    Username = reader["Username"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString(),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Gender = reader["Gender"].ToString(),
                    TypeOfIBD = reader["TypeOfIBD"].ToString(),
                    ProfilePicture = reader["ProfilePicture"] != DBNull.Value ? reader["ProfilePicture"].ToString() : null
                };
                return user;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateReadOneCommandWithStoredProcedure(String spName, SqlConnection con, string email)
    {
        SqlCommand cmd = new SqlCommand(spName, con)
        {
            CommandTimeout = 10,
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@email", email);

        return cmd;
    }

    public bool AddFriend(int userId, int friendId)
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

        cmd = new SqlCommand("spAddFriend", con)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@FriendId", friendId);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
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
                con.Close();
            }
        }
    }
    public List<User> GetFriends(int userId)
    {
        List<User> friends = new List<User>();
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in GetFriends: {ex.Message}", ex);
        }

        cmd = new SqlCommand("spGetFriends", con)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@UserId", userId);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                User friend = new User
                {
                    Id = dataReader.GetInt32(dataReader.GetOrdinal("UserID")),
                    Username = dataReader["Username"].ToString(),
                    ProfilePicture = dataReader["ProfilePicture"].ToString()
                };
                friends.Add(friend);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in GetFriends: {ex.Message}", ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

        return friends;
    }
}


