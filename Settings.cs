using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;


namespace CardMaker;


public class Settings
{
    private readonly IConfigurationRoot configRoot;

    private AzureOpenAISettings azureOpenAI;
    private OpenAISettings openAI;

    public AzureOpenAISettings AzureOpenAI => this.azureOpenAI ??= this.GetSettings<Settings.AzureOpenAISettings>();
    public OpenAISettings OpenAI => this.openAI ??= this.GetSettings<Settings.OpenAISettings>();

    public class OpenAISettings
    {
        public string ChatModel { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }

    public class AzureOpenAISettings
    {
        public string ChatModelDeployment { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }

    public TSettings GetSettings<TSettings>() =>
        this.configRoot.GetRequiredSection(typeof(TSettings).Name).Get<TSettings>()!;

    public Settings()
    {
        this.configRoot =
            new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .Build();
    }
}

public class ContextUtils
{
    private OpenAIAssistantAgent agent;
    private string threadId;

    public ContextUtils(OpenAIAssistantAgent agent, string threadId)
    {
        this.agent = agent;
        this.threadId = threadId;
    }

    public async void backchannel(string input)
    {
        agent.AddChatMessageAsync(threadId, new ChatMessageContent(AuthorRole.System, input));
    }

    public async Task streamCompletion()
    {
        await foreach (StreamingChatMessageContent response in agent.InvokeStreamingAsync(threadId))
        {
            Console.Write($"{response.Content}");
        }
        Console.WriteLine();
    }
}