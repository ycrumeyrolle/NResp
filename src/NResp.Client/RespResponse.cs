namespace NResp.Client
{
    using System.Collections.Generic;

    public class RespResponse
    {
        public RespResponse()
        {
            this.Responses = new List<string>();
        }

        public bool Success { get; set; }
        
        public string Response { get; set; }

        public long Length { get; set; }

        public string Key { get; set; }

        public IList<string> Responses { get; private set; }
    }
}
