using System.ClientModel;
using System.Text;
using CardMaker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;
using OpenAI.Files;
using OpenAI.VectorStores;



// Set the console encoding to UTF-8 to support Unicode characters
Console.OutputEncoding = Encoding.UTF8;

// Load embedded resources
string cardMaker, toolsList, yamlPersona;
cardMaker = ContextUtils.EmbeddedResource("Agent_Prompt_v2.md");
toolsList = ContextUtils.EmbeddedResource("Tools.txt");
yamlPersona = ContextUtils.EmbeddedResource("Persona_Prompt.yaml");

// Load configuration from environment variables or user secrets.
Settings settings = new();

// prerequisites for the OpenAI Assistant
OpenAIClientProvider clientProvider =
    OpenAIClientProvider.ForAzureOpenAI(
        new ApiKeyCredential(settings.AzureOpenAI.ApiKey),
        new Uri(settings.AzureOpenAI.Endpoint));

Console.WriteLine("Creating store...");
VectorStoreClient storeClient = clientProvider.Client.GetVectorStoreClient();
CreateVectorStoreOperation operation = await storeClient.CreateVectorStoreAsync(waitUntilCompleted: true);
string storeId = operation.VectorStoreId;        

/* uncomment if you want to upload a file to the vector store
Dictionary<string, OpenAIFile> fileReferences = [];
OpenAIFileClient fileClient = clientProvider.Client.GetOpenAIFileClient();
Stream stream = ContextUtils.EmbeddedResourceStream("platform-engineering.pdf");
OpenAIFile fileInfo = await fileClient.UploadFileAsync(stream, "platform-engineering.pdf", FileUploadPurpose.Assistants);
await storeClient.AddFileToVectorStoreAsync(storeId, fileInfo.Id, waitUntilCompleted: true);
fileReferences.Add(fileInfo.Id, fileInfo);
*/

var builder = Kernel.CreateBuilder();
//builder.Services.AddSingleton(ContextUtils.getLoggerFactory());

builder.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o-2",
    apiKey: settings.AzureOpenAI.ApiKey,
    endpoint: settings.AzureOpenAI.Endpoint,
    modelId: "gpt-4o" // Optional name of the underlying model if the deployment name doesn't match the model name
   );


var kernel = builder.Build();
Tools tools = new();
kernel.Plugins.AddFromObject(tools);

var function = kernel.CreateFunctionFromPromptYaml(yamlPersona);
kernel.Plugins.AddFromFunctions("quote_tool", [function]);

OpenAIAssistantAgent agent =
            await OpenAIAssistantAgent.CreateAsync(
                clientProvider,
                new OpenAIAssistantDefinition(settings.AzureOpenAI.ChatModelDeployment)
                {
                    Name = "Card Maker",
                    Instructions = cardMaker,
                    Temperature = 0.4f,
                    TopP = 0.9f,
                    EnableFileSearch = true,
                    VectorStoreId = storeId
                },
                kernel);

string threadId = await agent.CreateThreadAsync();
var utils = new ContextUtils(agent, threadId);

if (!Console.IsOutputRedirected)
{
    Console.Clear();
}
try
{
    // provide additional grounding before starting the conversation
    await utils.backchannel("here are the tools we will use exclusively for this mapping exercise");
    await utils.backchannel(toolsList);
    await utils.AddChatMessageAsync("Hello! What are you good at and why are we here?");
    string input;
    while ((input = Console.ReadLine()) is not null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.Write(">");
            continue;
        }
        if (input.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase))
        {
            break;
        }
        input += utils.formatPrompt();
        await utils.AddChatMessageAsync(input);
    }
}
finally
{
    Console.WriteLine();
    Console.WriteLine("Cleaning-up...");
    await Task.WhenAll(
        [
            agent.DeleteThreadAsync(threadId),
            agent.DeleteAsync(),
            storeClient.DeleteVectorStoreAsync(storeId),
            //..fileReferences.Select(fileReference => fileClient.DeleteFileAsync(fileReference.Key))
        ]);
}
