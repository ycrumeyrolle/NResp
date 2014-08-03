namespace NResp.Client
{
    using System;
    using System.Linq;

    public class LpopCommand : RespCommand
    {
        public LpopCommand(string listName)
            : base("LPOP", new[] { ValidateListName(listName) })
        {
        }

        public LpopCommand(params string[] listNames)
            : base("LPOP", ValidateListNames(listNames))
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