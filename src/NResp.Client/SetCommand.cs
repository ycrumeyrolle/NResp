namespace NResp.Client
{
    using System;
    using System.Globalization;

    public class SetCommand : RespCommand
    {
        public SetCommand(string key, string value)
            : base("SET", new[] { ValidateKey(key), ValidateValue(value) })
        {
        }

        public SetCommand(string key, string value, TimeSpan expires)
            : base("SET", new[] { ValidateKey(key), ValidateValue(value), "PX", ConvertExpiration(expires) })
        {
        }

        private static string ConvertExpiration(TimeSpan expires)
        {
            if (expires <= TimeSpan.Zero)
            {
                throw new ArgumentException("Expiration must be greater than zero.", "expires");
            }

            return expires.TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
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