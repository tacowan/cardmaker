﻿using System.ClientModel;
using System.Text;
using CardMaker;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;


// Set the console encoding to UTF-8 to support Unicode characters
Console.OutputEncoding = Encoding.UTF8;

// Load embedded resources
string cardMaker, toolsList, yamlPersona;
cardMaker = ContextUtils.EmbeddedResource("Agent_Prompt.txt");
toolsList = ContextUtils.EmbeddedResource("Tools.txt");
yamlPersona = ContextUtils.EmbeddedResource("Persona_Prompt.yaml");

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
Tools tools = new();
kernel.Plugins.AddFromObject(tools);

var function = kernel.CreateFunctionFromPromptYaml(yamlPersona);
kernel.Plugins.AddFromFunctions("Quote_tool", [function]);

OpenAIAssistantAgent agent =
            await OpenAIAssistantAgent.CreateAsync(
                clientProvider,
                new OpenAIAssistantDefinition(settings.AzureOpenAI.ChatModelDeployment)
                {
                    Name = "Card Maker",
                    Instructions = cardMaker,
                    Temperature = 1f,
                    TopP = 0f
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
        ]);
}
