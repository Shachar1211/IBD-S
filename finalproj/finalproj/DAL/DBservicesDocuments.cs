using finalproj.BL;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace finalproj.DAL
{
    public class DBservicesDocuments
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

        public Documents Insert(Documents document)
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

            cmd = CreateInsertCommandWithStoredProcedure("spInsertDocument", con, document);
            SqlParameter outputParam = cmd.Parameters.Add("@NewDocumentId", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                int newId = Convert.ToInt32(outputParam.Value);
                document.DocumentId = newId; // Update the DocumentId property of the Document object
                return document;
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

        private SqlCommand CreateInsertCommandWithStoredProcedure(string spName, SqlConnection con, Documents document)
        {
            SqlCommand cmd = new SqlCommand(spName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10;
            cmd.Parameters.AddWithValue("@FileId", (object)document.FileId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UserId", document.UserId);
            cmd.Parameters.AddWithValue("@DocumentName", document.DocumentName);
            cmd.Parameters.AddWithValue("@DocumentPath", document.DocumentPath);
            cmd.Parameters.AddWithValue("@UploadDate", document.UploadDate);

            return cmd;
        }

        public List<Documents> Read(int userID)
        {
            List<Documents> documents = new List<Documents>();
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
            cmd = new SqlCommand("spReadDocumentsForUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userID); // הוספת הפרמטר לפקודה

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Documents document = new Documents
                    {
                        DocumentId = dataReader.GetInt32(0),
                        FileId = dataReader.IsDBNull(1) ? (int?)null : dataReader.GetInt32(1),
                        UserId = dataReader.GetInt32(2),
                        DocumentName = dataReader.GetString(3),
                        DocumentPath = dataReader.GetString(4),
                        UploadDate = dataReader.GetDateTime(5),
                    };
                    documents.Add(document);
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
            return documents;
        }

        public List<Documents> ReadByFileId(int fileId)
        {
            List<Documents> documents = new List<Documents>();
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in ReadByFileId: {ex.Message}", ex);
            }

            cmd = new SqlCommand("spReadDocumentsForFile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FileId", fileId); // הוספת הפרמטר לפקודה

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Documents document = new Documents
                    {
                        DocumentId = dataReader.GetInt32(0),
                        FileId = dataReader.IsDBNull(1) ? (int?)null : dataReader.GetInt32(1),
                        UserId = dataReader.GetInt32(2),
                        DocumentName = dataReader.GetString(3),
                        DocumentPath = dataReader.GetString(4),
                        UploadDate = dataReader.GetDateTime(5),
                    };
                    documents.Add(document);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error in ReadByFileId: {ex.Message}", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return documents;
        }


        public bool Delete(int documentId)
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

            cmd = new SqlCommand("spDeleteDocument", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DocumentId", documentId);

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
