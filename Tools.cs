using System.ComponentModel;
using Microsoft.SemanticKernel;
using TextCopy;


namespace CardMaker;

public class Tools
{ 
 
    [KernelFunction("save_cards")]
    [Description("Save the card suit to a local file.")]
    [return: Description("The full card suit in JSON format.")]
    public async Task<string> save_cards(string json_document, [Description("card suit name, such as hearts, clubs, spades, or diamonds")] string suit)
    {
        string filePath = $"{suit}_cards.json";
        await File.WriteAllTextAsync(filePath, json_document);
        var d = Directory.GetCurrentDirectory();
        return "File successfully created at " + d + Path.DirectorySeparatorChar +  filePath;
    }

    [KernelFunction("copy_to_clipboard")]
    [Description("Copy the text to the clipboard.")]
    [return: Description("A message indicating the text was copied.")]
    public async Task<string> copy_to_clipboard(string text)
    {
        await ClipboardService.SetTextAsync(text);
        return "Text copied to clipboard.";
    }

}

