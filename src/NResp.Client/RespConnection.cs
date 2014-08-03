namespace NResp.Client
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class RespConnection : IRespConnection
    {
        private const string CrLf = "\r\n";

        public RespConnection(string host, ushort port)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }

            this.Host = host;
            this.Port = port;
        }

        /// <summary>
        /// Gets the host;
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        public ushort Port { get; private set; }
        
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
            TcpClient client = new TcpClient(this.Host, this.Port);
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
                        response.Success = false;
                        response.Response = await reader.ReadLineAsync();
                        break;
                    case '$':
                        response.Success = true;
                        response.Length = Convert.ToInt64(await reader.ReadLineAsync());
                        if (response.Length != -1)
                        {
                            response.Response = await reader.ReadLineAsync();
                        }

                        break;
                    case '+':
                        response.Success = true;
                        response.Response = await reader.ReadLineAsync();

                        break;
                }

                return response;
            }
        }
    }
}
