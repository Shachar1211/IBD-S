using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using finalproj.BL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class DBservicesChatGPTResponse

{
    public DBservicesChatGPTResponse()
    {

    }
    public SqlConnection connect(String conString)
    {
        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString(conString);
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }
    public int Insert(ChatGPTResponse chatGPTResponse)
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

        cmd = CreateGPTInsertCommandWithStoredProcedure("spInsertChatGPTResponse", con, chatGPTResponse);  // יצירת הפקודה

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
              
                con.Close();
            }
        }

    }

    private SqlCommand CreateGPTInsertCommandWithStoredProcedure(string spName, SqlConnection con, ChatGPTResponse chatGPTResponse)
    {
        SqlCommand cmd = new SqlCommand(spName, con); //יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה

        cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
        cmd.CommandTimeout = 10;

        // הוספת פרמטרים עם ערכים לאובייקט הפקודה
        cmd.Parameters.AddWithValue("@Answer", chatGPTResponse.Answer);
        string userParamsJson = JsonConvert.SerializeObject(chatGPTResponse.UserParams);
        cmd.Parameters.AddWithValue("@UserParams", userParamsJson);

        return cmd;
    }

    public List<ChatGPTResponse> Read()
    {

        SqlConnection con;
        SqlCommand cmd;
        List<ChatGPTResponse> chatGPTResponses = new List<ChatGPTResponse>();

        try
        {
            con = connect("myProjDB"); // יצירת החיבור
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateReadCommandWithStoredProcedureWithoutParameters("spReadChatGPTResponses", con);//יצירת הפקודה

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); //ביצוע הפקודה

            while (dataReader.Read()) //המרת המידע שהתקבל מהדאטה בייס לאובייקט המתאים
            {
                ChatGPTResponse c = new ChatGPTResponse();
                c.Answer = dataReader["Answer"].ToString();
                c.ResponseID = dataReader.GetInt32(0);
                UserParams userParams = new UserParams(
                percentageOfDisability: dataReader.GetInt32(1),
                disabilityRating: dataReader.GetBoolean(2),
                ibdType: dataReader["IbdType"].ToString(),
                userId: dataReader.GetInt32(3),
                student: dataReader.GetBoolean(2)
            );
                c.UserParams = userParams;
                chatGPTResponses.Add(c);
            }
            return chatGPTResponses;
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
    private SqlCommand CreateReadCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); 

        cmd.Connection = con;              

        cmd.CommandText = spName;      

        cmd.CommandTimeout = 10;          

        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        return cmd;
    }


}
