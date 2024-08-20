using finalproj.DAL;

namespace finalproj.BL
{
    public class Chat
    {
        private int chatId;
        private int senderId;
        private int recipientId;
        private string contenct;
        private DateTime sendDate;
        private bool attachedFile;
        private string user2Username;
        private string user2ProfilePicture;
        private bool areFriends;

        public Chat(int chatId, int senderId, int recipientId, string contenct, DateTime sendDate, bool attachedFile)
        {
            ChatId = chatId;
            SenderId = senderId;
            RecipientId = recipientId;
            Contenct = contenct;
            SendDate = sendDate;
            AttachedFile = attachedFile;
        }
        public Chat() { }

        public int ChatId { get => chatId; set => chatId = value; }
        public int SenderId { get => senderId; set => senderId = value; }
        public int RecipientId { get => recipientId; set => recipientId = value; }
        public string Contenct { get => contenct; set => contenct = value; }
        public DateTime SendDate { get => sendDate; set => sendDate = value; }
        public bool AttachedFile { get => attachedFile; set => attachedFile = value; }
        public string User2ProfilePicture { get => user2ProfilePicture; set => user2ProfilePicture = value; }
        public bool AreFriends { get => areFriends; set => areFriends = value; }
        public string User2Username { get => user2Username; set => user2Username = value; }

        public bool Insert()
        {
            if (this.attachedFile == true)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles");
                // יצירת תיקיה להעלאת התמונות אם לא קיימת
                Directory.CreateDirectory(path);

                // החלפת תווים לא חוקיים משם הקובץ
                string sanitizedFileName = this.sendDate.ToString("yyyyMMdd_HHmmss") + ".jpg";

                // שמירת התמונה
                System.IO.File.WriteAllBytes(Path.Combine(path, sanitizedFileName), Convert.FromBase64String(this.contenct));
                this.contenct = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/{sanitizedFileName}";
            }

            DBservicesChat dbService = new DBservicesChat();
            return dbService.Insert(this);
        }

        public List<Chat> readFullChat(int userid1,int userid2)
        {
            DBservicesChat dbService = new DBservicesChat();
            return dbService.GetFullChat(userid1, userid2);
        }

        public List<Chat> ReadLatestChat(int userID)
        {
            DBservicesChat dbService = new DBservicesChat();
            return dbService.GetLatestMessages(userID);
        }

        public List<Chat> readFullChatFromDate(int userid1, int userid2, DateTime startDate)
        {
            DBservicesChat dbService = new DBservicesChat();
            return dbService.GetFullChatFromDate(userid1, userid2, startDate);
        }

    }
}
