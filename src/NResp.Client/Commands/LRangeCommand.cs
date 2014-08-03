namespace NResp.Client.Commands
{
    public class LRangeCommand : RangeCommand
    {
        public LRangeCommand(string key, int start, int stop)
            : base("LRANGE", key, start, stop)
        {
        }
    }
}