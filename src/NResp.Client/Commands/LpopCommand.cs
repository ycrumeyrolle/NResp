namespace NResp.Client.Commands
{
    public class LPopCommand : PopCommand
    {
        public LPopCommand(string listName)
            : base("LPOP", listName)
        {
        }

        public LPopCommand(params string[] listNames)
            : base("LPOP", listNames)
        {
        }
    }
}