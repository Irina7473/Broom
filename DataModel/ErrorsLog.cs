using System;

namespace DataModel
{
    public class ErrorsLog
    {
        public int Id { get; set; }
        public int IdTypeEvent { get; set; }
        public string DateTimeEvent { get; set; }
        public string User { get; set; }
        public string ErrorsMessage { get; set; }

        public ErrorsLog(int id, int idtypeevent, string datetimeevent, string user, string errorsmessage)
        {
            Id = id;
            IdTypeEvent = idtypeevent;
            DateTimeEvent = datetimeevent;
            User = user;
            ErrorsMessage = errorsmessage;
        }
       
        public ErrorsLog() { }
    }
}