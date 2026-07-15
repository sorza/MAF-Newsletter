namespace Newsletter.Core.Agents.Abstractions
{
    public interface IAgent<in TData, TResponse>
    {
        Task<TResponse> RunAsync(TData data, CancellationToken cancellationToken = default);
    }
}
