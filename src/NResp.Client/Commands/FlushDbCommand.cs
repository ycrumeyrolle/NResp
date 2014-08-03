namespace NResp.Client.Commands
{
    public class FlushDbCommand : RespCommand
    {
        public FlushDbCommand()
            : base("FLUSHDB")
        {
        }
    }
}