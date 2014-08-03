namespace NResp.Client.Commands
{
    public class ExistsCommand : RespCommand
    {
        public ExistsCommand(string key)
            : base("EXISTS", new[] { key })
        {
        }
    }
}