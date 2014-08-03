namespace NResp.Client
{
    public class PingCommand : RespCommand
    {
        public PingCommand()
            : base("PING")
        {
        }
    }
}
