﻿using System.ClientModel;
using System.Text;
using CardMaker;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;



// Set the console encoding to UTF-8 to support Unicode characters
Console.OutputEncoding = Encoding.UTF8;

// Load embedded resources
string cardMaker, quotePersona, toolsList, yamlPersona;
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
Tools tools = new Tools(toolsList, kernel);
kernel.Plugins.AddFromObject(tools);
var function = kernel.CreateFunctionFromPrompt(yamlPersona);
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

try
{
    utils.backchannel(toolsList);
    // Generated by Copilot
    if (!Console.IsOutputRedirected)
    {
        Console.Clear();
    }

    await agent.AddChatMessageAsync(
        threadId, 
        new ChatMessageContent(AuthorRole.User, "Hello! What are you good at and why are we here?"+utils.formatPrompt()));
    await utils.streamCompletion();
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
        await agent.AddChatMessageAsync(threadId, new ChatMessageContent(AuthorRole.User, input));
        await utils.streamCompletion();
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




