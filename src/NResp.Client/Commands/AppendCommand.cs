namespace NResp.Client.Commands
{
    using System;

    public class AppendCommand : RespCommand
    {
        public AppendCommand(string key, string value)
            : base("APPEND", new[] { ValidateKey(key), ValidateValue(value) })
        {
        }
        
        private static string ValidateKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return key;
        }

        private static string ValidateValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return value;
        }
    }
}