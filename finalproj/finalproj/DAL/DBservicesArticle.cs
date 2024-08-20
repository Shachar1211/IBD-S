using finalproj.BL;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace finalproj.DAL
{
    public class DBservicesArticle
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

        public List<Article> GetAllArticles()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Article> articles = new List<Article>();

            try
            {
                con = connect("myProjDB"); // יצירת החיבור
                cmd = new SqlCommand("spGetAllArticles ", con); // יצירת האובייקט של הפקודה והקצאת שם הפרוצדורה השמורה
                cmd.CommandType = CommandType.StoredProcedure; // הגדרת סוג הפקודה כפרוצדורה שמורה
                cmd.CommandTimeout = 10;

             
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Article article = new Article
                    {
                      InfoId = (int)reader["InfoId"],
                      Picture = reader["Picture"].ToString(),
                      Header =reader["Header"].ToString(),
                      Contenct= reader["Contenct"].ToString(),
                      Link =reader["Link"].ToString()
                    };
                    articles.Add(article);
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

            return articles;
        }
    }
}
