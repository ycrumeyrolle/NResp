namespace NResp.Client.Commands
{
    using System;
    using System.Globalization;

    public class RangeCommand : RespCommand
    {
        public RangeCommand(string name, string key, int start, int stop)
            : base(name, new[] { ValidateKey(key), start.ToString(CultureInfo.InvariantCulture), stop.ToString(CultureInfo.InvariantCulture) })
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
    }
}
