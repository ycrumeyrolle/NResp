namespace NResp.Client.Commands
{
    public class RPopCommand : PopCommand
    {
        public RPopCommand(string listName)
            : base("RPOP", listName)
        {
        }

        public RPopCommand(params string[] listNames)
            : base("RPOP", listNames)
        {
        }
    }
}