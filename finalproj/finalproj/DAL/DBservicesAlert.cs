using finalproj.BL;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

public class DBservicesAlert
{
    public DBservicesAlert() { }

    public SqlConnection connect(string conString)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public Alert Insert(Alert alert)
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

        cmd = CreateInsertCommandWithStoredProcedure("spInsertAlert", con, alert);
        SqlParameter outputParam = cmd.Parameters.Add("@NewAlertId", SqlDbType.Int);
        outputParam.Direction = ParameterDirection.Output;

        try
        {
            cmd.ExecuteNonQuery();
            int newId = Convert.ToInt32(outputParam.Value);
            alert.AlertId = newId; // Update the AlertId property of the Alert object
            return alert;
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


private SqlCommand CreateInsertCommandWithStoredProcedure(string spName, SqlConnection con, Alert alert)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;

        cmd.Parameters.AddWithValue("@EventID", alert.EventId);
        cmd.Parameters.AddWithValue("@Aname", alert.Aname);
        cmd.Parameters.AddWithValue("@Arepeat", alert.Arepeat ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@AlertTime", alert.AlertTime);
        cmd.Parameters.AddWithValue("@Identifier", alert.Identifier);

        //SqlParameter outputParam = new SqlParameter("@NewAlertId", SqlDbType.Int)
        //{
        //    Direction = ParameterDirection.Output
        //};
        //cmd.Parameters.Add(outputParam);

        return cmd;
    }

    public List<Alert> ReadByEvent(int eventId)
    {
        List<Alert> alerts = new List<Alert>();
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

        cmd = CreateReadByEventCommandWithStoredProcedure("spReadAlertsByEvent", con, eventId);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader();
            try
            {
                while (dataReader.Read())
                {
                    Alert alert = new Alert
                    {
                        AlertId = dataReader.GetInt32(0),
                        EventId = dataReader.GetInt32(1),
                        Aname = dataReader["Aname"].ToString(),
                        Arepeat = dataReader["Arepeat"].ToString(),
                        AlertTime = dataReader.GetDateTime(4),
                        Identifier = dataReader["Identifier"].ToString()

                    };
                    alerts.Add(alert);
                }
            }
            finally
            {
                dataReader.Close();
            }

            return alerts;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error in ReadByEvent: {ex.Message}", ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateReadByEventCommandWithStoredProcedure(string spName, SqlConnection con, int eventId)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;

        cmd.Parameters.AddWithValue("@EventID", eventId);

        return cmd;
    }

    public Alert ReadOne(int alertId)
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

        cmd = CreateReadOneCommandWithStoredProcedure("spReadOneAlert", con, alertId);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader();
            try
            {
                if (dataReader.Read())
                {
                    Alert alert = new Alert
                    {
                        AlertId = dataReader.GetInt32(0),
                        EventId = dataReader.GetInt32(1),
                        Aname = dataReader["Aname"].ToString(),
                        Arepeat = dataReader["Arepeat"].ToString(),
                        AlertTime = dataReader.GetDateTime(4),
                        Identifier = dataReader["Identifier"].ToString()
                    };
                    return alert;
                }
            }
            finally
            {
                dataReader.Close();
            }

            return null;
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
    }

    private SqlCommand CreateReadOneCommandWithStoredProcedure(string spName, SqlConnection con, int alertId)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;

        cmd.Parameters.AddWithValue("@AlertID", alertId);

        return cmd;
    }

    public int Update(Alert alert)
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

        cmd = CreateUpdateCommandWithStoredProcedure("spUpdateAlert", con, alert);

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

    private SqlCommand CreateUpdateCommandWithStoredProcedure(string spName, SqlConnection con, Alert alert)
    {
        SqlCommand cmd = new SqlCommand(spName, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = 10;

        cmd.Parameters.AddWithValue("@AlertID", alert.AlertId);
        cmd.Parameters.AddWithValue("@EventID", alert.EventId);
        cmd.Parameters.AddWithValue("@Aname", alert.Aname);
        cmd.Parameters.AddWithValue("@Arepeat", alert.Arepeat ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@AlertTime", alert.AlertTime);
        cmd.Parameters.AddWithValue("@Identifier", alert.Identifier);

        return cmd;
    }

    public bool Delete(Alert alert)
    {
        SqlConnection con = null;
        try
        {
            con = connect("myProjDB");
            SqlCommand cmd = new SqlCommand("spDeleteAlert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AlertID", alert.AlertId);

            var result = cmd.ExecuteNonQuery();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting alert: " + ex.Message);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public bool DeleteAlertForEvent(Alert alert)
    {
        SqlConnection con = null;
        try
        {
            con = connect("myProjDB");
            SqlCommand cmd = new SqlCommand("spDeleteAlertForEvent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EventID", alert.EventId);

            var result = cmd.ExecuteNonQuery();
            return result > 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting alerts for event: " + ex.Message);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<Alert> GetAlertsByUserId(int userId)
    {
        List<Alert> alertsList = new List<Alert>();
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        try
        {
            cmd = new SqlCommand("spGetAlertsByUserId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userId);

            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Alert alert = new Alert
                {
                    AlertId = dataReader.GetInt32(0),
                    EventId = dataReader.GetInt32(1),
                    Aname = dataReader["Aname"].ToString(),
                    Arepeat = dataReader["Arepeat"].ToString(),
                    AlertTime = dataReader.GetDateTime(4),
                    Identifier = dataReader["Identifier"].ToString()

                };
                alertsList.Add(alert);
            
        }
            return alertsList;
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
}
