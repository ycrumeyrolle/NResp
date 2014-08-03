namespace NResp.Client
{
    using System;
    using System.Globalization;

    public class SetCommand : RespCommand
    {
        public SetCommand(string key, string value)
            : base("SET", new[] { key, value })
        {
        }

        public SetCommand(string key, string value, TimeSpan expires)
            : base("SET", new[] { key, value, "PX", ConvertExpiration(expires) })
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
    }
}