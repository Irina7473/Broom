namespace Logger
{
    public interface ILogger
    {
        public void RecordToLog(string typeevent, string message) { }
        public void ReadTheLog() { }
        public void ClearLog() { }
    }
}