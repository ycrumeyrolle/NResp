namespace NResp.Client.Commands
{
    using System.Globalization;

    public class SelectCommand : RespCommand
    {
        public SelectCommand(int index)
            : base("SELECT", new[] { index.ToString(CultureInfo.InvariantCulture) })
        {
        }
    }
}