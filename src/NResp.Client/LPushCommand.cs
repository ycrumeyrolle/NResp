namespace NResp.Client
{
    using System;
    using System.Linq;

    public class LPushCommand : RespCommand
    {
        public LPushCommand(string listName, string value)
            : base("LPUSH", new[] {listName, value })
        {
        }

        public LPushCommand(string listName, params string[] values)
            : base("LPUSH", new [] {ValidateListName(listName)}.Concat(values).ToArray())
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