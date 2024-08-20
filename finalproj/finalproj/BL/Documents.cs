using finalproj.DAL;

namespace finalproj.BL
{
    public class Documents
    {
        private int documentId;
        public int? FileId { get; set; } // הערך הזה יכול להיות null

        private int userId;
        private string documentName;
        private string documentPath;
        private DateTime uploadDate;

        public Documents(int documentId, int fileId, int userId, string documentName, string documentPath, DateTime uploadDate)
        {
            DocumentId = documentId;
            FileId = fileId;
            UserId = userId;
            DocumentName = documentName;
            DocumentPath = documentPath;
            UploadDate = uploadDate;
        }

        public Documents(int documentId) {
            DocumentId = documentId;

        }
        public Documents() { }
        public int DocumentId { get => documentId; set => documentId = value; }
        public int UserId { get => userId; set => userId = value; }
        public string DocumentName { get => documentName; set => documentName = value; }
        public string DocumentPath { get => documentPath; set => documentPath = value; }
        public DateTime UploadDate { get => uploadDate; set => uploadDate = value; }


        public Documents Insert()
        {
            // יצירת נתיב לתיקייה להעלאת הקבצים אם היא לא קיימת
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles");
            Directory.CreateDirectory(path);

            if (!string.IsNullOrEmpty(this.DocumentPath))
            {
                try
                {
                    // שמירת הקובץ בפורמט PDF
                    string filePath = Path.Combine(path, $"{this.documentName}.pdf");
                    byte[] fileBytes = Convert.FromBase64String(this.DocumentPath);
                    System.IO.File.WriteAllBytes(filePath, fileBytes);

                    // יצירת ה-URL לגישה לקובץ
                    this.DocumentPath = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/{this.documentName}.pdf";
                }
                catch (Exception ex)
                {
                    // טיפול בשגיאה במידה וישנה
                    Console.WriteLine("Error saving file: " + ex.Message);
                }
            }

            // שמירה לבסיס הנתונים
            DBservicesDocuments dbService = new DBservicesDocuments();
            return dbService.Insert(this);
        }

        public List<Documents> GetDocumetsByUserId(int userId)
        {
            DBservicesDocuments dbService = new DBservicesDocuments();
            return dbService.Read(userId);
        }

        public List<Documents> GetDocumentsByFileId(int fileId)
        {
            DBservicesDocuments dbService = new DBservicesDocuments();
            return dbService.ReadByFileId(fileId);
        }
        public bool Delete()
        {
            DBservicesDocuments dbService = new DBservicesDocuments();
            return dbService.Delete(this.DocumentId);
        }
    }
}
