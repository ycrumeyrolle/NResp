namespace NResp.Client
{
    using System.Threading;
    using System.Threading.Tasks;
    using NResp.Client.Commands;

    public interface IRespConnection
    {
        Task<RespResponse> SendAsync(RespCommand command, CancellationToken cancellationToken);
    }
}