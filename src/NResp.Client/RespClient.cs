namespace NResp.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RespClient
    {
        private readonly IRespConnection connection;

        public RespClient(string hostName, int port)
            : this(new RespConnection(hostName, port))
        {
        }

        public RespClient(IRespConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.connection = connection;
        }

        public async Task<string> GetAsync(string key, CancellationToken cancellationToken)
        {
            GetCommand get = new GetCommand(key);

            var result = await this.connection.SendAsync(get, cancellationToken);
            if (!result.Success)
            {
                throw new RespException(result.Response);
            }

            return result.Response;
        }

        public Task SetAsync(string key, string value, CancellationToken cancellationToken)
        {
            SetCommand command = new SetCommand(key, value);
            return this.SetAsyncCore(command, cancellationToken);
        }

        public Task SetAsync(string key, string value, TimeSpan expires, CancellationToken cancellationToken)
        {
            SetCommand command = new SetCommand(key, value, expires);

            return this.SetAsyncCore(command, cancellationToken);
        }

        private async Task SetAsyncCore(SetCommand command, CancellationToken cancellationToken)
        {
            var result = await this.connection.SendAsync(command, cancellationToken);
            if (!result.Success)
            {
                throw new RespException(result.Response);
            }
        }
    }
}
