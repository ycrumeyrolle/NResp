namespace NResp.Client.Commands
{
    public class LPushCommand : PushCommand
    {
        public LPushCommand(string listName, string value)
            : base("RPUSH", listName, value)
        {
        }

        public LPushCommand(string listName, params string[] values)
            : base("RPUSH", listName, values)
        {
        }
    }
}