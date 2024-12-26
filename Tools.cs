using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;


namespace CardMaker;

public class Tools
{
    private readonly Kernel kernel;
    private readonly string prompt;
    private readonly string toolsList;
    // Generated by Copilot
    public Tools(string prompt, string toolsList, Kernel kernel)
    {
        this.kernel = kernel;
        this.prompt = prompt;
        this.toolsList = toolsList;
    }


    [KernelFunction("random_quote")]
    [Description("retrieve a random quoteyes.")]
    [return: Description("a random quote for a card.")]
    public async Task<string> random_quote(int card_number, string suit_theme, string role, string text)
    {
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory history = [];    
        history.AddSystemMessage(prompt);
        history.AddUserMessage($"Significance of {role}: {text}, share a quote of 3 sentences or less on how {role} achives the goals of {suit_theme}.");

        var response = await chatCompletionService.GetChatMessageContentAsync(
            history,
            kernel: kernel
        );
        return response.ToString();
    }


    [KernelFunction("save_cards")]
    [Description("Save the card suit to a local file.")]
    [return: Description("The full card suit in JSON format.")]
    public async Task<string> save_cards(string json_document, [Description("card suit name, such as hearts, clubs, spades, or diamonds")] string suit)
    {
        string filePath = $"{suit}_cards.json";
        await File.WriteAllTextAsync(filePath, json_document);
        return json_document;
    }

}

