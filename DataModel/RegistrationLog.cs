using System;

namespace DataModel
{
    public class RegistrationLog
    {
        public int Id { get; set; }
        public string TypeEvent { get; set; }
        public string DateTimeEvent { get; set; }
        public string User { get; set; }
        public string Message { get; set; }

        public RegistrationLog(int id, string typeevent, string datetimeevent, string user, string message)
        {
            Id = id;
            TypeEvent = typeevent;
            DateTimeEvent = datetimeevent;
            User = user;
            Message = message;
        }

        public RegistrationLog() { }
    }
}