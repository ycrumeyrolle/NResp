namespace NResp.Client.Commands
{
    using System;

    public class DelCommand : RespCommand
    {
        public DelCommand(string key)
            : base("DEL", new[] { ValidateKey(key) })
        {
        }

        public DelCommand(params string[] keys)
            : base("DEL", ValidateKeys(keys))
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

        private static string[] ValidateKeys(string[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == null)
                {
                    throw new ArgumentException("Keys can not be null", "keys");
                }
            }

            return keys;
        }
    }
}