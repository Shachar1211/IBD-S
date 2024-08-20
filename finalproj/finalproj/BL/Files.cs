using finalproj.DAL;

namespace finalproj.BL
{
    public class Files
    {
        private int filesId;
        private int userId;
        private string fileName;

        public Files(int filesId, int userId, string fileName)
        {
            FilesId = filesId;
            UserId = userId;
            FileName = fileName;
        }

        public Files() { }

        public Files(int filesId)
        {
            FilesId = filesId;
        }

        public int FilesId { get => filesId; set => filesId = value; }
        public int UserId { get => userId; set => userId = value; }
        public string FileName { get => fileName; set => fileName = value; }



        public List<Files> GetFilesByUserId(int userId)
        {
            DBservicesFiles dbService = new DBservicesFiles();
            return dbService.Read(userId);
        }

        public Files Insert()
        {
            DBservicesFiles dbService = new DBservicesFiles();
            return dbService.Insert(this);
        }

        public bool Delete()
        {
            DBservicesFiles dbService = new DBservicesFiles();
            return dbService.Delete(this.FilesId);
        }
        public Files Update()
        {
            DBservicesFiles dbService = new DBservicesFiles();
            return dbService.Update(this);
        }


    }
}
