using System.ClientModel;
using System.Reflection;
using System.Text;
using CardMaker;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;


// Set the console encoding to UTF-8 to support Unicode characters
Console.OutputEncoding = Encoding.UTF8;

// Load embedded resources
string cardMaker, quotePersona, toolsList;
cardMaker = EmbeddedResource("Prompt.txt");
quotePersona = EmbeddedResource("Prompt2.txt");
toolsList = EmbeddedResource("Tools.txt");


// Load configuration from environment variables or user secrets.
Settings settings = new();

OpenAIClientProvider clientProvider =
    OpenAIClientProvider.ForAzureOpenAI(
        new ApiKeyCredential(settings.AzureOpenAI.ApiKey), 
        new Uri(settings.AzureOpenAI.Endpoint));

var builder = Kernel.CreateBuilder();

builder.AddAzureOpenAIChatCompletion(
    deploymentName: settings.AzureOpenAI.ChatModelDeployment,
    apiKey: settings.AzureOpenAI.ApiKey,
    endpoint: settings.AzureOpenAI.Endpoint,
    modelId: "gpt-4o" // Optional name of the underlying model if the deployment name doesn't match the model name
   );


var kernel = builder.Build();
Tools tools = new Tools(quotePersona, toolsList, kernel);
kernel.Plugins.AddFromObject(tools);

OpenAIAssistantAgent agent =
            await OpenAIAssistantAgent.CreateAsync(
                clientProvider,
                new OpenAIAssistantDefinition(settings.AzureOpenAI.ChatModelDeployment)
                {
                    Name = "Card Maker",
                    Instructions = cardMaker,
                    Temperature = 0.5f,
                    TopP = 0.0f
                },
                kernel);

string threadId = await agent.CreateThreadAsync();
var utils = new ContextUtils(agent, threadId);

try
{
    
    bool isComplete = false;
    utils.backchannel(toolsList);
    await agent.AddChatMessageAsync(threadId, new ChatMessageContent(AuthorRole.User, "Hello!"));
    do
    {
        await utils.streamCompletion();
        Console.Write("\n> ");

        // read the input, if it's good, send it to the agent
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            continue;
        }
        if (input.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        {
            isComplete = true;
            break;
        }
        input += "Format your response to be directly printed in a CLI console. Avoid Markdown.  Freely use emoticans or ANSI escape sequences to colorize or highlight text.";

        await agent.AddChatMessageAsync(threadId, new ChatMessageContent(AuthorRole.User, input));
        Console.WriteLine();


    } while (!isComplete);
}
finally
{
    Console.WriteLine();
    Console.WriteLine("Cleaning-up...");
    await Task.WhenAll(
        [
        agent.DeleteThreadAsync(threadId),
        agent.DeleteAsync(),
        ]);
}



static string EmbeddedResource(string filename)
{
    var assembly = Assembly.GetExecutingAssembly();
    using (var stream = assembly.GetManifestResourceStream($"cardmaker.Resources.{filename}"))
    using (var reader = new StreamReader(stream))
    {
        return reader.ReadToEnd();
    }
}

