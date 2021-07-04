using System;

namespace DataModel
{
    class ErrorsLog
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