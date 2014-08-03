namespace NResp.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRespConnection
    {
        Task<RespResponse> SendAsync(RespCommand command, CancellationToken cancellationToken);
    }
}