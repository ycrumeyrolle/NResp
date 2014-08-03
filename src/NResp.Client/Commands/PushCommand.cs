namespace NResp.Client.Commands
{
    using System;
    using System.Linq;

    public class PushCommand : RespCommand
    {
        public PushCommand(string name, string listName, string value)
            : base(name, new[] { listName, value })
        {
        }

        public PushCommand(string name, string listName, params string[] values)
            : base(name, new[] { ValidateListName(listName) }.Concat(values).ToArray())
        {
        }

        private static string ValidateListName(string listName)
        {
            if (listName == null)
            {
                throw new ArgumentNullException("listName");
            }

            return listName;
        }
    }
}