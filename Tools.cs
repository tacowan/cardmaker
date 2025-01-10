using System.ComponentModel;
using System.Net;
using Microsoft.SemanticKernel;
using QRCoder;
using TextCopy;
using System.Drawing;



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

    [KernelFunction("load_cards")]
    [Description("Load the card suit from a local file.")]
    [return: Description("card suit as JSON document.")]
    public async Task<string> load_card(string fileName)
    {
        // load the card suit from a local file
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        var json = await File.ReadAllTextAsync(filePath);
        return json;
    }

    [KernelFunction("create_qr_code")]
    [Description("Create a QR code from the bing query text.")]
    [return: Description("The path to the QR code image.")]
    public async Task<string> create_qr_code(string query, int card_id)
    {
        // http encode query
        query = WebUtility.UrlEncode(query);
        var s = $"https://www.bing.com/search?q={query}++site:learn.microsoft.com&shm=cr&form=DEEPSH";
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(s, QRCodeGenerator.ECCLevel.Q);
        
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(20); 
        var qrCodePath = Path.Combine(Directory.GetCurrentDirectory(), $"{card_id}.png");
        qrCodeImage.Save(qrCodePath);
        return qrCodePath;
    }
    

}

