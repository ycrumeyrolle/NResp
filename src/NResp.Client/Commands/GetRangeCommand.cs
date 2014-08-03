namespace NResp.Client.Commands
{
    public class GetRangeCommand : RangeCommand
    {
        public GetRangeCommand(string key, int start, int stop)
            : base("GETRANGE", key, start, stop)
        {
        }
    }
}