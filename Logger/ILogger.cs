namespace Logger
{
    public interface ILogger
    {
        public void RecordToLog(string typeevent, string message) { }
        public string ReadTheLog() { return ""; }
        public void ClearLog() { }
    }
}