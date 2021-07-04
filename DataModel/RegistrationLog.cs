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

    public class SuccessLog
    {
        public int Id { get; set; }
        public int IdRegistration { get; set; }
        public string SuccessMessage { get; set; }
        
        public SuccessLog(int id, int idregistration, string successmessage)
        {
            Id = id;
            IdRegistration = idregistration;
            SuccessMessage = successmessage;
        }

        public SuccessLog() { }
    }

    public class ErrorsLog
    {
        public int Id { get; set; }
        public int IdRegistration { get; set; }
        public string ErrorsMessage { get; set; }

        public ErrorsLog(int id, int idregistration, string errorsmessage)
        {
            Id = id;
            IdRegistration = idregistration;
            ErrorsMessage = errorsmessage;
        }

        public ErrorsLog() { }
    }
}