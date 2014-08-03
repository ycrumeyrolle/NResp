namespace NResp.Client
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class RespConnection : IRespConnection
    {
        private const string CrLf = "\r\n";

        public RespConnection(string hostName, int port)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }

            this.HostName = hostName;
            this.Port = port;
        }

        /// <summary>
        /// Gets the host name;
        /// </summary>
        public string HostName { get; private set; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public int Port { get; private set; }

        public async Task<RespResponse> SendAsync(RespCommand command, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();

            // Array 
            sb.Append('*').Append(command.Arguments.Count + 1).Append(CrLf);

            // Bulk string
            sb.Append('$').Append(command.Name.Length).Append(CrLf);

            // The command
            sb.Append(command.Name).Append(CrLf);

            for (int i = 0; i < command.Arguments.Count; i++)
            {
                var argument = command.Arguments[i];
                sb.Append('$').Append(argument.Length).Append(CrLf);
                sb.Append(argument).Append(CrLf);
            }

            byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
            TcpClient client = new TcpClient(this.HostName, this.Port);
            var stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length, cancellationToken);
            
            using (StreamReader reader = new StreamReader(stream, Encoding.ASCII, true, 8192, true))
            {
                char[] resultCode = new char[1];

                await reader.ReadAsync(resultCode, 0, resultCode.Length);
                RespResponse response = new RespResponse();
                switch (resultCode[0])
                {
                    case '-':
                        return await ReadError(reader);

                    case '$':
                        return await ReadBulk(reader);
                    
                    case '+':
                        return await ReadString(reader);

                    case ':':
                        return await ReadInteger(reader);

                    case '*':
                        response.Success = true;
                        int arrayLength = Convert.ToInt32(await reader.ReadLineAsync());
                        if (arrayLength != 2)
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The server reply an array of {0} elements. Expected : 2 elements", arrayLength));
                        }

                        // length value unchecked
                        await reader.ReadLineAsync();
                        var result = await reader.ReadLineAsync();
                        break;

                    default:
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Return code '{0}' is not unknow.", resultCode[0]));
                }

                return response;
            }
        }

        private static async Task<RespResponse> ReadInteger(StreamReader reader)
        {
            RespResponse response = new RespResponse();
            response.Success = true;
            response.Length = Convert.ToInt64(await reader.ReadLineAsync());
            return response;
        }

        private static async Task<RespResponse> ReadString(StreamReader reader)
        {
            RespResponse response = new RespResponse();
            response.Success = true;
            response.Response = await reader.ReadLineAsync();
            return response;
        }

        private static async Task<RespResponse> ReadBulk(StreamReader reader)
        {
            RespResponse response = new RespResponse();
            response.Success = true;
            response.Length = Convert.ToInt64(await reader.ReadLineAsync());
            if (response.Length != -1)
            {
                response.Response = await reader.ReadLineAsync();
            }

            return response;
        }

        private static async Task<RespResponse> ReadError(StreamReader reader)
        {
            RespResponse response = new RespResponse();
            response.Success = false;
            response.Response = await reader.ReadLineAsync();
            return response;
        }
    }
}
