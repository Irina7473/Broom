using System;

namespace DataModel
{
    class SuccessLog
    {
        public int Id { get; set; }
        public int IdTypeEvent { get; set; }
        public string DateTimeEvent { get; set; }
        public string User { get; set; }
        public string SuccessMessage { get; set; }

        public SuccessLog(int id, int idtypeevent, string datetimeevent, string user, string successmessage)
        {
            Id = id;
            IdTypeEvent = idtypeevent;
            DateTimeEvent = datetimeevent;
            User = user;
            SuccessMessage = successmessage;
        }

        public SuccessLog() { }
    }
}