using finalproj.BL;
using finalproj.DAL;


namespace finalproj.BL
{
    public class Mail
    {
        int mailid;
        int userid;
        string picture;
        string username;
        int senderUserId;
        DateTime sendDate;
        int forumQustionId;
        string forumSubject;
        string forumContent;
        int calendarEventId;
        string calendaerEventName;
        string calendarEventLocation;
        string calenderEventStartTime;
        bool mailFromCalander;



        public Mail()
        {

        }

        public Mail(int mailid, int userid, string picture, string username, int senderUserId, DateTime sendDate, int forumQustionId, string forumSubject, string forumContent, int calendarEventId, string calendaerEventName, string calendarEventLocation, bool mailFromCalander, string calenderEventStartTime)
        {
            Mailid = mailid;
            Userid = userid;
            Picture = picture;
            Username = username;
            SenderUserId = senderUserId;
            SendDate = sendDate;
            ForumQustionId = forumQustionId;
            ForumSubject = forumSubject;
            ForumContent = forumContent;
            CalendarEventId = calendarEventId;
            CalendaerEventName = calendaerEventName;
            CalendarEventLocation = calendarEventLocation;
            MailFromCalander = mailFromCalander;
            CalenderEventStartTime = calenderEventStartTime;
        }

        public int Mailid { get => mailid; set => mailid = value; }
        public int Userid { get => userid; set => userid = value; }
        public string Picture { get => picture; set => picture = value; }
        public string Username { get => username; set => username = value; }
        public int SenderUserId { get => senderUserId; set => senderUserId = value; }
        public DateTime SendDate { get => sendDate; set => sendDate = value; }
        public int ForumQustionId { get => forumQustionId; set => forumQustionId = value; }
        public string ForumSubject { get => forumSubject; set => forumSubject = value; }
        public string ForumContent { get => forumContent; set => forumContent = value; }
        public int CalendarEventId { get => calendarEventId; set => calendarEventId = value; }
        public string CalendaerEventName { get => calendaerEventName; set => calendaerEventName = value; }
        public string CalendarEventLocation { get => calendarEventLocation; set => calendarEventLocation = value; }
        public bool MailFromCalander { get => mailFromCalander; set => mailFromCalander = value; }
        public string CalenderEventStartTime { get => calenderEventStartTime; set => calenderEventStartTime = value; }

        public bool Insert()
        {
            if(this.MailFromCalander==true)
            {
                this.Picture = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/adaptive-icon.png";
            }
            DBservicesMail dbService = new DBservicesMail();
            return dbService.Insert(this);
            
        }
        public List<Mail> ReadMail(int userID)
        {
            DBservicesMail dbService = new DBservicesMail();
            return dbService.GetMailsByUserId(userID);
        }

        public bool DeleteByQuestion(int QuestionID)
        {
            DBservicesMail dbService = new DBservicesMail();
            return dbService.DeleteByQuestion(QuestionID);
        }

        public bool DeleteByCalender(int CalanderID)
        {
            DBservicesMail dbService = new DBservicesMail();
            return dbService.DeleteByCalender(CalanderID);
        }
    }

}
