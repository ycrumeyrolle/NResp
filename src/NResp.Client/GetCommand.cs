namespace NResp.Client
{
    public class GetCommand : RespCommand
    {
        public GetCommand(string key)
            : base("GET", new[] { key })
        {
        }
    }
}