using finalproj.BL;
using System.Data;
using System.Data.SqlClient;

namespace finalproj.DAL
{
    public class DBservicesFiles
    {

        public SqlConnection connect(String conString)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString(conString);
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        public List<Files> Read(int userID)
        {
            List<Files> files = new List<Files>();
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in Read: {ex.Message}", ex);
            }
            cmd = new SqlCommand("spReadFilesForUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userID); // הוספת הפרמטר לפקודה

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Files file = new Files
                    {
                        FilesId = dataReader.GetInt32(0),
                        UserId = dataReader.GetInt32(1),
                        FileName = dataReader["FileName"].ToString(),
                    };
                    files.Add(file);
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
            return files;
        }

        public Files Insert(Files files)
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

            cmd = CreateInsertCommandWithStoredProcedure("spInsertFile", con, files);
            SqlParameter outputParam = cmd.Parameters.Add("@NewFileId", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                int newId = Convert.ToInt32(outputParam.Value);
                files.FilesId = newId; // Update the FileId property of the File object
                return files;
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

        private SqlCommand CreateInsertCommandWithStoredProcedure(string spName, SqlConnection con, Files files)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10;
            cmd.Parameters.AddWithValue("@UserId", files.UserId);
            cmd.Parameters.AddWithValue("@FileName", files.FileName);

            return cmd;
        }

        public bool Delete(int filesId)
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

            cmd = new SqlCommand("spDeleteFile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FilesId", filesId);

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

        public Files Update(Files file)
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

            cmd = CreateUpdateCommandWithStoredProcedure("spUpdateFile", con, file);

            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    file.FilesId = reader.GetInt32(reader.GetOrdinal("FilesId"));
                    file.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                    file.FileName = reader.GetString(reader.GetOrdinal("FileName"));
                }
                return file;
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

        private SqlCommand CreateUpdateCommandWithStoredProcedure(string spName, SqlConnection con, Files file)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10;

            cmd.Parameters.AddWithValue("@FileID", file.FilesId);
            cmd.Parameters.AddWithValue("@UserID", file.UserId);
            cmd.Parameters.AddWithValue("@FileName", file.FileName);
            return cmd;
        }
    }
}


