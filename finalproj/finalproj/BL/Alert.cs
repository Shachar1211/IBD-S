using System;
using System.Collections.Generic;

namespace finalproj.BL
{
    public class Alert
    {
        private int alertId;
        private int eventId;
        private string aname;
        private string arepeat;
        private DateTime alertTime;
        private string identifier;

        public static List<Alert> Alerts = new List<Alert>();

        public Alert(int eventId, string aname, string arepeat, string identifier)
        {
            EventId = eventId;
            Aname = aname;
            Arepeat = arepeat;
            Identifier = identifier;    
        }

        public Alert() { }

        public int AlertId { get => alertId; set => alertId = value; }
        public int EventId { get => eventId; set => eventId = value; }
        public string Aname { get => aname; set => aname = value; }
        public string Arepeat { get => arepeat; set => arepeat = value; }
        public DateTime AlertTime { get => alertTime; set => alertTime = value; }
        public string Identifier { get => identifier; set => identifier = value; }

        public Alert Insert(int userId)
        {
            CalendarEvent calendarEvent = new CalendarEvent().ReadOne(EventId);
            if (calendarEvent != null)
            {
                this.AlertTime = CalculateAlertTime(calendarEvent.StartTime, Arepeat);
            }

            DBservicesAlert dbs = new DBservicesAlert();

            
            return dbs.Insert(this);
        }

        public List<Alert> ReadByEvent(int eventId)
        {
            DBservicesAlert dbs = new DBservicesAlert();
            return dbs.ReadByEvent(eventId);
        }

        public Alert ReadOne(int alertId)
        {
            DBservicesAlert dbs = new DBservicesAlert();
            return dbs.ReadOne(alertId);
        }

        public int Update()
        {
            DBservicesAlert dbs = new DBservicesAlert();
            return dbs.Update(this);
        }

        public bool Delete()
        {
            DBservicesAlert dbs = new DBservicesAlert();
            return dbs.Delete(this);
        }

        public List<Alert> GetAlertsByUserId(int userId)
        {
            DBservicesAlert dbs = new DBservicesAlert();
            return dbs.GetAlertsByUserId(userId);
        }

        //private DateTime CalculateAlertTime(DateTime eventStartTime, string arepeat)
        //{ 
        //    int minutesBeforeEvent;
        //    if (int.TryParse(arepeat, out minutesBeforeEvent))
        //    {
        //        return eventStartTime.AddMinutes(-minutesBeforeEvent);
        //    }
        //    throw new ArgumentException("Invalid repeat time format.");
        //}

        private DateTime CalculateAlertTime(DateTime eventStartTime, string arepeat)
        {
            int minutesBeforeEvent = ConvertToMinutes(arepeat);
            return eventStartTime.AddMinutes(-minutesBeforeEvent);
        }

        private int ConvertToMinutes(string repeat)
        {
            if (repeat.Contains("דקות"))
            {
                // Extract the number of minutes from the string
                int minutes = int.Parse(repeat.Split(' ')[0]);
                return minutes;
            }
            else if (repeat.Contains("שעות"))
            {
                // Extract the number of hours and convert to minutes
                int hours = int.Parse(repeat.Split(' ')[0]);
                return hours * 60;
            }
            else if (repeat.Contains("ימים"))
            {
                // Extract the number of days and convert to minutes
                int days = int.Parse(repeat.Split(' ')[0]);
                return days * 24 * 60;
            }
            else if (repeat.Contains("שבועות"))
            {
                // Extract the number of weeks and convert to minutes
                int weeks = int.Parse(repeat.Split(' ')[0]);
                return weeks * 7 * 24 * 60;
            }
            else if (repeat.Contains("אף פעם"))
            {
                int never = 0;
                return never;
            }
            else
            {
                throw new ArgumentException("Invalid repeat time format.");
            }
        }
    }
}
