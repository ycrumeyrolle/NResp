namespace NResp.Client.Commands
{
    public class GetCommand : RespCommand
    {
        public GetCommand(string key)
            : base("GET", new[] { key })
        {
        }
    }
}