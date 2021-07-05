using System;

namespace DataModel
{
    public class TotalLog
    {
        public int Id { get; set; }
        public int IdTypeEvent { get; set; }
        public string DateTimeEvent { get; set; }
        public string User { get; set; }
        public string TotalMessage { get; set; }

        public TotalLog(int id, int idtypeevent, string datetimeevent, string user, string totalmessage)
        {
            Id = id;
            IdTypeEvent = idtypeevent;
            DateTimeEvent = datetimeevent;
            User = user;
            TotalMessage = totalmessage;
        }

        public TotalLog() { }
    }
}