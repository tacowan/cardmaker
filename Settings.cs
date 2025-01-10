using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;
using ConsoleTextFormat;
using B = ConsoleTextFormat.Fmt.Bold;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;



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
    private StringBuilder sb = new StringBuilder();
    private string threadId;

    public ContextUtils(OpenAIAssistantAgent agent, string threadId)
    {
        this.agent = agent;
        this.threadId = threadId;
    }

    public string formatPrompt()
    {
        return $"""
        Please format your response using UNICODE escape codes for terminal output. For example:

        Use {B.fgWhi}{B.bgBla}Heading{Fmt.clear} for top-level headings.

        Use {Fmt.b}text{Fmt._b} for sub-headings.
            e.g. for sub-heading sedan under heading cars, {Fmt.b}Sedan{Fmt._b}  
        
        Use - for bullet points.
        
        Use {Fmt.ul}Underline{Fmt._ul} for underlining.
        Use {Fmt.b}Bold{Fmt._b} for bolding.
        Freely use emoticans. 
        Always respond using plain text, no matter the context. Markdown is never allowed.       
        """;
    }

    public static string EmbeddedResource(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (var stream = assembly.GetManifestResourceStream($"cardmaker.Resources.{filename}"))
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    public static Stream EmbeddedResourceStream(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream($"cardmaker.Resources.{filename}");
    }

    public static ILoggerFactory getLoggerFactory()
    {
        var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService("TelemetryConsoleQuickstart");

        // Enable model diagnostics with sensitive data.
        AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

        var traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("Microsoft.SemanticKernel*")
            .AddConsoleExporter()
            .Build();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            // Add OpenTelemetry as a logging provider
            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resourceBuilder);
                options.AddConsoleExporter();
                // Format log messages. This is default to false.
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;
            });
            builder.SetMinimumLevel(LogLevel.None);
        });
        return loggerFactory;
    }

    public async Task backchannel(string input)
    {
        await agent.AddChatMessageAsync(threadId, new ChatMessageContent(AuthorRole.Assistant, input));
    }

    public async Task AddChatMessageAsync(string input)
    {
        await agent.AddChatMessageAsync(threadId, new ChatMessageContent(AuthorRole.User, input + formatPrompt()));
        await streamCompletion();
    }
    public async Task streamCompletion()
    {
        await foreach (StreamingChatMessageContent response in agent.InvokeStreamingAsync(threadId))
        {
            if (response.Content == null)
            {
                continue;
            }
            foreach (char c in response.Content)
            {
                if (c != '\\')
                    Console.Write(c);
            }
        }
        Console.WriteLine();
        Console.Write("\n> ");

    }

}