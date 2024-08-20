using System;
using System.Collections.Generic;

namespace finalproj.BL
{
    public class CalendarEvent
    {
        private int eventId;
        private int userId;
        private DateTime startTime;
        private DateTime endTime;
        private string name;
        private string location;
        private string repeat;  // מתוקן לאות קטנה
        private int day;
        private int month;
        private int year;
        private int parentEvent;

        public static List<CalendarEvent> CalendarEvents = new List<CalendarEvent>();

        public CalendarEvent(int userId, DateTime startTime, DateTime endTime, string name, string location, string repeat, int day, int month, int year, int parentEvent)
        {
            UserId = userId;
            StartTime = startTime;
            EndTime = endTime;
            Name = name;
            Location = location;
            Repeat = repeat;
            Day = day;
            Month = month;
            Year = year;
            ParentEvent = parentEvent;  
        }

        public CalendarEvent() { }
        public CalendarEvent(int parentEvent) 
        {
            ParentEvent = parentEvent;

        }



        public int EventId { get => eventId; set => eventId = value; }
        public int UserId { get => userId; set => userId = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
        public string Name { get => name; set => name = value; }
        public string Location { get => location; set => location = value; }
        public string Repeat { get => repeat; set => repeat = value; }
        public int Day { get => day; set => day = value; }
        public int Month { get => month; set => month = value; }
        public int Year { get => year; set => year = value; }
        public int ParentEvent { get => parentEvent; set => parentEvent = value; }

        public CalendarEvent Insert()
        {
            DBservicesEvent dbs = new DBservicesEvent();
            return dbs.Insert(this);
        }

        public List<CalendarEvent> Read(int userId)
        {
            DBservicesEvent dbs = new DBservicesEvent();
            return dbs.Read(userId);
        }

        public CalendarEvent ReadOne(int eventId)
        {
            DBservicesEvent dbs = new DBservicesEvent();
            return dbs.ReadOne(eventId);
        }

        public int Update()
        {
            DBservicesEvent dbs = new DBservicesEvent();
            return dbs.Update(this);
        }

        public bool Delete()
        {
            DBservicesEvent dbs = new DBservicesEvent();
            return dbs.Delete(this);
        }

        public bool DeleteByParentEvent()
        {
            DBservicesEvent dbs = new DBservicesEvent();
            return dbs.DeleteByParentEvent(this.ParentEvent);
        }
    }
}
