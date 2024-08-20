using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using finalproj.BL;
using Microsoft.Extensions.Configuration;

public class DBservicesEvent
{
    public DBservicesEvent()
    {
    }

    public SqlConnection connect(string conString)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public CalendarEvent Insert(CalendarEvent calendarEvent)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = CreateInsertCommandWithStoredProcedure("spInsertCalendarEvent", con, calendarEvent);
        SqlParameter outputParam = cmd.Parameters.Add("@NewEventId", SqlDbType.Int);
        outputParam.Direction = ParameterDirection.Output;

        try
        {
            cmd.ExecuteNonQuery();
            int newId = Convert.ToInt32(outputParam.Value);
            calendarEvent.EventId = newId; // Update the EventId property of the CalendarEvent object
            return calendarEvent;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateInsertCommandWithStoredProcedure(string spName, SqlConnection con, CalendarEvent calendarEvent)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;
        cmd.Parameters.AddWithValue("@UserID", calendarEvent.UserId);
        cmd.Parameters.AddWithValue("@StartTime", calendarEvent.StartTime);
        cmd.Parameters.AddWithValue("@EndTime", calendarEvent.EndTime);
        cmd.Parameters.AddWithValue("@Name", calendarEvent.Name);
        cmd.Parameters.AddWithValue("@Location", calendarEvent.Location ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Repeat", calendarEvent.Repeat ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Day", calendarEvent.Day);
        cmd.Parameters.AddWithValue("@Month", calendarEvent.Month);
        cmd.Parameters.AddWithValue("@Year", calendarEvent.Year);
        cmd.Parameters.AddWithValue("@ParentEvent", calendarEvent.ParentEvent);


        return cmd;
    }

    public List<CalendarEvent> Read(int userId)
    {
        List<CalendarEvent> calendarEvents = new List<CalendarEvent>();
        string connectionString = GetConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("spReadCalendarEvents", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userId);

            try
            {
                con.Open();
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        CalendarEvent calendarEvent = new CalendarEvent
                        {
                            EventId = dataReader.GetInt32(0),
                            UserId = dataReader.GetInt32(1),
                            StartTime = dataReader.GetDateTime(2),
                            EndTime = dataReader.GetDateTime(3),
                            Name = dataReader["Name"].ToString(),
                            Location = dataReader["Location"].ToString(),
                            Repeat = dataReader["Repeat"].ToString(),
                            Day = dataReader.GetInt32(7),
                            Month = dataReader.GetInt32(8),
                            Year = dataReader.GetInt32(9),
                            ParentEvent = dataReader.GetInt32(10)
                        };
                        calendarEvents.Add(calendarEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in Read: {ex.Message}", ex);
            }
        }

        return calendarEvents;
    }

    public CalendarEvent ReadOne(int eventId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = CreateReadOneCommandWithStoredProcedure("spReadOneCalendarEvent", con, eventId);

        try
        {
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    CalendarEvent calendarEvent = new CalendarEvent
                    {
                        EventId = dataReader.GetInt32(0),
                        UserId = dataReader.GetInt32(1),
                        StartTime = dataReader.GetDateTime(2),
                        EndTime = dataReader.GetDateTime(3),
                        Name = dataReader["Name"].ToString(),
                        Location = dataReader["Location"].ToString(),
                        Repeat = dataReader["Repeat"].ToString(),
                        Day = dataReader.GetInt32(7),
                        Month = dataReader.GetInt32(8),
                        Year = dataReader.GetInt32(9),
                        ParentEvent = dataReader.GetInt32(10)
                    };
                    return calendarEvent;
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in ReadOne: {ex.Message}", ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

        return null;
    }

    private SqlCommand CreateReadOneCommandWithStoredProcedure(string spName, SqlConnection con, int eventId)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;

        cmd.Parameters.AddWithValue("@EventID", eventId);

        return cmd;
    }

    public int Update(CalendarEvent calendarEvent)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = CreateUpdateCommandWithStoredProcedure("spUpdateCalendarEvent", con, calendarEvent);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateUpdateCommandWithStoredProcedure(string spName, SqlConnection con, CalendarEvent calendarEvent)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;

        cmd.Parameters.AddWithValue("@EventID", calendarEvent.EventId);
        cmd.Parameters.AddWithValue("@UserID", calendarEvent.UserId);
        cmd.Parameters.AddWithValue("@StartTime", calendarEvent.StartTime);
        cmd.Parameters.AddWithValue("@EndTime", calendarEvent.EndTime);
        cmd.Parameters.AddWithValue("@Name", calendarEvent.Name);
        cmd.Parameters.AddWithValue("@Location", calendarEvent.Location ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Repeat", calendarEvent.Repeat ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Day", calendarEvent.Day);
        cmd.Parameters.AddWithValue("@Month", calendarEvent.Month);
        cmd.Parameters.AddWithValue("@Year", calendarEvent.Year);
        cmd.Parameters.AddWithValue("@ParentEvent", calendarEvent.ParentEvent);


        return cmd;
    }

    public bool Delete(CalendarEvent calendarEvent)
    {
        SqlConnection con = null;
        try
        {
            con = connect("myProjDB");
            SqlCommand cmd = new SqlCommand("spDeleteCalendarEvent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EventID", calendarEvent.EventId);
            //cmd.Parameters.AddWithValue("@UserID", calendarEvent.UserId);

            var result = cmd.ExecuteNonQuery();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting calendar event: " + ex.Message);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    public bool DeleteByParentEvent(int parentEvent)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = CreateDeleteByParentEventCommandWithStoredProcedure("spDeleteEventsByParentEvent", con, parentEvent);

        try
        {
            var result = cmd.ExecuteNonQuery();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateDeleteByParentEventCommandWithStoredProcedure(string spName, SqlConnection con, int parentEvent)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;
        cmd.Parameters.AddWithValue("@ParentEvent", parentEvent);
        return cmd;
    }

    private string GetConnectionString()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        return configuration.GetConnectionString("myProjDB");
    }
}
