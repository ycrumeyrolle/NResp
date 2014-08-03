namespace NResp.Client.Commands
{
    public class RPushCommand : PushCommand
    {
        public RPushCommand(string listName, string value)
            : base("RPUSH", listName, value)
        {
        }

        public RPushCommand(string listName, params string[] values)
            : base("RPUSH", listName, values)
        {
        }
    }
}