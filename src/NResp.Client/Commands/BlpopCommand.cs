namespace NResp.Client.Commands
{
    using System;
    using System.Linq;

    public class BlpopCommand : RespCommand
    {
        public BlpopCommand(string listName)
            : base("BLPOP", new[] { ValidateListName(listName), "0" })
        {
        }

        public BlpopCommand(params string[] listNames)
            : base("BLPOP", ValidateListNames(listNames).Concat(new[] { "0" }).ToArray())
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

        private static string[] ValidateListNames(string[] listNames)
        {
            if (listNames == null)
            {
                throw new ArgumentNullException("listNames");
            }

            for (int i = 0; i < listNames.Length; i++)
            {
                if (listNames[i] == null)
                {
                    throw new ArgumentException("List names can not be null", "listNames");
                }
            }

            return listNames;
        }
    }
}