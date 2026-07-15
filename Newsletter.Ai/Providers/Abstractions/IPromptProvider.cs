namespace Newsletter.Ai.Providers.Abstractions
{
    public interface IPromptProvider
    {
        Task<string> GetPromptAsync(string agentName);
    }
}
