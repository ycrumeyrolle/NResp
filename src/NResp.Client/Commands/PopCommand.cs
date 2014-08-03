namespace NResp.Client.Commands
{
    using System;

    public class PopCommand : RespCommand
    {
        public PopCommand(string name, string listName)
            : base(name, new[] { ValidateListName(listName) })
        {
        }

        public PopCommand(string name, params string[] listNames)
            : base(name, ValidateListNames(listNames))
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