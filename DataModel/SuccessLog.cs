using System;

namespace DataModel
{
    class SuccessLog
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
}